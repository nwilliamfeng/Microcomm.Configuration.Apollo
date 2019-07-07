using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.Ctrip.Framework.Apollo;
using Com.Ctrip.Framework.Apollo.ConfigAdapter;
using Com.Ctrip.Framework.Apollo.Enums;
using Com.Ctrip.Framework.Apollo.Internals;
using Com.Ctrip.Framework.Apollo.Spi;

namespace Microcomm.Configuration.Apollo
{
    //public class ApolloTest
    //{

    //    public async void Load()
    //    {
    //        IConfigManager
    //        var config =await ApolloConfigurationManager.GetConfig("auth");
    //        var value = config.GetProperty("auth_base_url","");
    //    }
    //}

    public static class ConfigurationManagerFactory
    {
        public static IConfigManager Manager { get; } = new DefaultConfigManager(new DefaultConfigRegistry(), new ConfigRepositoryFactory(new ConfigUtil()));

        
        public static Task<IConfig> GetAppConfig() => GetConfig("application");

         
        public static Task<IConfig> GetConfig(string namespaceName)
        {
            if (string.IsNullOrEmpty(namespaceName)) throw new ArgumentException("message", nameof(namespaceName));

            return Manager.GetConfig(namespaceName);
        }

        
        public static Task<IConfig> GetConfig(  params string[] namespaces) => GetConfig((IEnumerable<string>)namespaces);

        
        public static async Task<IConfig> GetConfig( IEnumerable<string> namespaces)
        {
            if (namespaces == null) throw new ArgumentNullException(nameof(namespaces));

            return new MultiConfig(await Task.WhenAll(namespaces.Reverse().Distinct().Select(GetConfig)).ConfigureAwait(false));
        }
    }
}
