using AzureCognitiveServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using templibrary;

namespace ConsoleApp_STD
{
    class Program
    {
        const string PATH = "D:\\Users\\bahid\\Desktop\\Images\\handwriting.jpg";
        const string PATH_2 = "D:\\Users\\bahid\\Desktop\\Images\\harry-meghan-15.jpg";


        static void Main(string[] args)
        {
            TextToSpeechManager textToSpeechManager = new TextToSpeechManager();

            var result = textToSpeechManager
                .GenerateSpeechWithDefaultSettings("This works! This works!", SpeakOut: true)
                .GetAwaiter()
                .GetResult();

            Console.WriteLine(result.BytesData == null);


        }
    }
}
