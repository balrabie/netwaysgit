using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AzureServices;

namespace ConsoleAppnetcore
{
    class Program
    {
        const string output_folder = @"D:\Users\bahid\Desktop\Output";

        const string image_house = @"D:\Users\bahid\Desktop\Images\house_yard.jpg";

        const string image_printed = @"D:\Users\bahid\Desktop\Images\printedText.jpg";

        const string image_handwritten = @"D:\Users\bahid\Desktop\Images\handwriting.jpg";

        const string image_people = @"D:\Users\bahid\Desktop\Images\harry-meghan-15.jpg";


        static void Main(string[] args)
        {
            Console.WriteLine("testing ConsoleAppnetcore:");

            //TestComputerVisionApi().Wait();

            //TestFaceApi().Wait();

            TestSpellChecker().Wait();

            TestBingSearchApi().Wait();
        }

        private static async Task TestSpellChecker()
        {
            string text = "I wiill be their in five minuetes";

            string result = (await SpellingManager.SpellCheck(text)).ToString();

            WriteToFile(text + "\n ==> \n" + result, "SpellingChecker", ".txt");
        }

        private static async Task TestFaceApi()
        {
            Console.WriteLine("TestFaceApi: START");

            var result_1 = (await FaceDetectorManager.DetectFaces(image_people)).ToString();

            WriteToFile(result_1, "Faces");

            Console.WriteLine("TestFaceApi: END");
        }


        private static async Task TestComputerVisionApi()
        {
            Console.WriteLine("TestComputerVision: START");

            var result_1 = (await ComputerVisionManager.AnalyzeImage(image_house)).ToString();

            var result_2 = (await ComputerVisionManager.MakeOCRRequest(image_printed)).ToString();

            var result_3 = (await ComputerVisionManager.ReadHandwrittenText(image_handwritten)).ToString();


            WriteToFile(result_1, "Analysis");

            WriteToFile(result_2, "Printed");

            WriteToFile(result_3, "Handwritten");

            Console.WriteLine("TestComputerVision: END");
        }


        private static async Task TestBingSearchApi()
        {
            Func<Dictionary<string, string>, string> HeaderToString = delegate (Dictionary<string, string> headers)
             {
                 StringBuilder result = new StringBuilder();

                 foreach (var header in headers)
                 {
                     string dashedLine = "\n" + String.Concat(Enumerable.Repeat("-", 10)) + "\n";
                     result.Append(dashedLine + header.Key + "\n ==> \n" + header.Value + dashedLine);
                 }

                 return result.ToString();
             };

            //var result_WebSearch = await SearchManager.BingWebSearch("Roadster Lebanon");
            var result_ImgSearch = await SearchManager.BingImageSearch("Dog");

            //string json_1 = result_WebSearch.jsonResult.ToString();
            //string header_1 = HeaderToString(result_WebSearch.relevantHeaders);

            string json_2 = result_ImgSearch.jsonResult.ToString();
            string header_2 = HeaderToString(result_ImgSearch.relevantHeaders);

            //WriteToFile(json_1, "WebSearch");
            //WriteToFile(header_1, "WebSearchHeaders", ".txt");

            WriteToFile(json_2, "imagesearch");
            WriteToFile(header_2, "imagesearchheaders", ".txt");
        }


        /// <summary>
        /// Creates a file, and writes to it some text.
        /// The file is created locally.
        /// </summary>
        /// <param name="text">The input text.</param>
        /// <param name="file_name">Name of the file to be created.</param>
        /// <param name="extension">The extension (is optional; default = .Json).</param>
        private static void WriteToFile
            (string text, string file_name, string extension = ".Json")
        {
            using (StreamWriter sw = File.CreateText(output_folder + @"\" + file_name + extension))
            {
                sw.WriteLine(text);
            }
        }
    }
}
