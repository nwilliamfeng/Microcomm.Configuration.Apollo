using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Microcomm.Configuration.Apollo
{
    public class ApolloConfigurationSection:ConfigurationSection
    {
        private static ApolloConfigurationSection instance ;

        public static ApolloConfigurationSection Current 
            => instance??(instance=ConfigurationManager.GetSection("apollo") as ApolloConfigurationSection);

        [ConfigurationProperty("appId", IsRequired = true)]
        public string AppId
        {
            get => base["appId"] as string; 
            set => base["appId"] = value;
        }

        [ConfigurationProperty("env", IsRequired = false)]
        public string Env
        {
            get =>string.IsNullOrEmpty( base["env"] as string)? "dev":base["env"] as string;
            set => base["env"] = value;
        }

        [ConfigurationProperty("namespaces", IsRequired = true)]
        public string NamespacesString
        {
            get => base["namespaces"] as string;
            set => base["namespaces"] = value;
        }


        [ConfigurationProperty("metaServer", IsRequired = true)]
        public string MetaServer
        {
            get => base["metaServer"] as string;
            set => base["metaServer"] = value;
        }


        public IEnumerable<string> Namespaces => string.IsNullOrEmpty(NamespacesString) ? new string[] { } : NamespacesString.Split(',');
       
    }
}
