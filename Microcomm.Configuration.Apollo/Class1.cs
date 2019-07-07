using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.Ctrip.Framework.Apollo;
using Com.Ctrip.Framework.Apollo.ConfigAdapter;
using Com.Ctrip.Framework.Apollo.Enums;

namespace Microcomm.Configuration.Apollo
{
    public class ApolloTest
    {
         
        public async void Load()
        {
           var config =await ApolloConfigurationManager.GetConfig("auth");
            var value = config.GetProperty("auth_base_url","");
        }
    }
}
