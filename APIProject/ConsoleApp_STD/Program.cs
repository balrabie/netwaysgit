using AzureCognitiveServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp_STD
{
    class Program
    {
        const string PATH = "D:\\Users\\bahid\\Desktop\\Images\\handwriting.jpg";
        const string PATH_2 = "D:\\Users\\bahid\\Desktop\\Images\\harry-meghan-15.jpg";


        static void Main(string[] args)
        {
            SearchManager sm = new SearchManager();

            var result = sm.GetBingWebSearch("what is c hash").GetAwaiter().GetResult();


        }
    }
}
