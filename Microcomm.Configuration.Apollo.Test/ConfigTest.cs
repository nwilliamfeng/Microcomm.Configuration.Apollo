using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Microcomm.Configuration.Apollo.Test
{
    public  class ConfigTest
    {
        public ConfigTest()
        {
            ApolloConfigManager.Current.ConfigChanged += Current_ConfigChanged;
        }

        private void Current_ConfigChanged(object sender, ConfigChangedEventArgs e)
        {
            PrintTitle("ConfigChange Event Raised...");
            var lst = e.ChangedItems.Select(x => JsonConvert.SerializeObject(x)).ToList();
            for (int i = 0; i < lst.Count; i++)
                Console.WriteLine($"{i}: {lst[i]}");
        }

        public void PrintConfigValue(string key)
        {
            Console.WriteLine();
            var strs = key.Split(',');
           var value = ApolloConfigManager.Current.GetConfigValue(strs[0],strs.Length==1?"application":strs[1]);
            Console.WriteLine($"the value of the  key {key} is: {value}");
            Console.WriteLine();
        }

        public void PrintSectionInfo()
        {
            Console.WriteLine();
            PrintTitle("print config section info...");
            Console.WriteLine($"appid: {ApolloConfigurationSection.Current.AppId}");
            Console.WriteLine($"namespaces: {ApolloConfigurationSection.Current.NamespacesString}");
            Console.WriteLine($"env: {ApolloConfigurationSection.Current.Env}");
            Console.WriteLine($"metaServer: {ApolloConfigurationSection.Current.MetaServer}");
            Console.WriteLine();
        }

        private void PrintTitle(string title)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(title);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
        }
    }
}
