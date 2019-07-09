using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microcomm.Configuration.Apollo.Test
{
    class Program
    {
        private static ConfigTest test = new ConfigTest();

        static void Main(string[] args)
        {
             test.PrintSectionInfo();
            

            while (true)
            {
               Console.WriteLine("Please enter key(the input format is {key},{namespace} if namespace is application please input only key):");
                var key = Console.ReadLine();
                test.PrintConfigValue(key);
               
            }

            Console.ReadLine();
        }

        
    }
}
