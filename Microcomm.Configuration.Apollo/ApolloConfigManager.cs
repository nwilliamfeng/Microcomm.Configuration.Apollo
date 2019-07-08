using Com.Ctrip.Framework.Apollo;
using Com.Ctrip.Framework.Apollo.Core;
using Com.Ctrip.Framework.Apollo.Internals;
using Com.Ctrip.Framework.Apollo.Util;
using Com.Ctrip.Framework.Apollo.Model;
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
        private readonly IConfig _config;


        private ApolloConfigManager()
        {
            var namespaces = ApolloConfigurationSection.Current.Namespaces; //默认从配置文件里找，如果没有加载application
            if (namespaces == null || namespaces.Count() == 0)
                namespaces = new string[] { ConfigConsts.NamespaceApplication };

            ConfigUtil.AppSettings = new System.Collections.Specialized.NameValueCollection
            {
                ["Apollo.Env"] = ApolloConfigurationSection.Current.Env,
                ["Apollo.AppId"] = ApolloConfigurationSection.Current.AppId,
                ["Apollo.MetaServer"] = ApolloConfigurationSection.Current.MetaServer,
            };
            this._config = ApolloConfigurationManager.GetConfig(namespaces).Result;
            this.RegistEvent();
        }

        
        private void RegistEvent()
        {
            this._config.ConfigChanged +=(s,e)=>
            {
                var items=  e.Changes.Values.Select(x => new ConfigChangedItem(x));
                this.ConfigChanged?.Invoke(this, new ConfigChangedEventArgs(items));
            };
        }

        public event EventHandler<ConfigChangedEventArgs> ConfigChanged;
     

        public static ApolloConfigManager Current
        {
            get
            {
                if (instance == null)
                    instance = new ApolloConfigManager();
                return instance;
            }
        }


        public string GetConfigValue(string key) => this._config.GetProperty(key,"");
       
       

    }
}
