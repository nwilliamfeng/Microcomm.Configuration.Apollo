using Com.Ctrip.Framework.Apollo;
using Com.Ctrip.Framework.Apollo.Core;
using Com.Ctrip.Framework.Apollo.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microcomm.Configuration.Apollo
{
    /// <summary>
    /// Apollo配置管理器
    /// </summary>
    public sealed class ApolloConfigManager
    {
        private static ApolloConfigManager instance;
        private readonly IDictionary<string, IConfig> _configDic;

        private ApolloConfigManager()
        {
            var namespaces = ApolloConfigurationSection.Current.Namespaces.Distinct(); //默认从配置文件里找，如果没有加载application,此处需要去重
            if (namespaces == null || namespaces.Count() == 0)
                namespaces = new string[] { ConfigConsts.NamespaceApplication };

            ConfigUtil.AppSettings = new System.Collections.Specialized.NameValueCollection
            {
                ["Apollo.Env"] = ApolloConfigurationSection.Current.Env,
                ["Apollo.AppId"] = ApolloConfigurationSection.Current.AppId,
                ["Apollo.MetaServer"] = ApolloConfigurationSection.Current.MetaServer,
            };
            _configDic = new Dictionary<string, IConfig>();
            namespaces.ToList().ForEach(np => _configDic[np] = ApolloConfigurationManager.GetConfig(np).Result);                    
            this.RegistEvent();
        }


        private void RegistEvent()
        {
            this._configDic.Values.ToList().ForEach(config =>
            {
                config.ConfigChanged += (s, e) =>
                {
                    var items = e.Changes.Values.Select(x => new ConfigChangedItem(x));
                    this.ConfigChanged?.Invoke(this, new ConfigChangedEventArgs(items));
                };
            });
           
        }

        public event EventHandler<ConfigChangedEventArgs> ConfigChanged;


        public static ApolloConfigManager Current => instance ?? (instance = new ApolloConfigManager());


        public string GetConfigValue(string key,string nameSpace= ConfigConsts.NamespaceApplication) => this._configDic[nameSpace]?.GetProperty(key, "");

        public T GetJsonConfigValue<T>(string key, string nameSpace = ConfigConsts.NamespaceApplication)
            where T:class,new() 
        {
            var str = this.GetConfigValue(key,nameSpace);
            if (string.IsNullOrEmpty(str))
                return null;
            return JsonConvert.DeserializeObject<T>(str);
        }
 
    }
}