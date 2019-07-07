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
        [ConfigurationProperty("appId", IsRequired = true)]
        public string AppId
        {
            get => base["appId"] as string; 
            set => base["appId"] = value;
        }

        [ConfigurationProperty("namespaces", IsRequired = true)]
        public IEnumerable<String> Namespaces
        {
            get => base["namespaces"].ToString().Split(',');
            set => base["namespaces"] = value == null ? null : string.Join(",", value);
        }
    }
}
