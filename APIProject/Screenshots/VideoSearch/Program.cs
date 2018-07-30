using AzureCognitiveServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using templibrary;
using System.Threading.Tasks;


namespace ConsoleApp_STD
{
    class Program
    {
        const string PATH = @"D:\Users\bahid\Desktop\myPrograms\437 Project\videos\london.mp4";


        static void Main(string[] args)
        {
            var result = VideoManager.CheckVideoForMatches(PATH, "person", 11).GetAwaiter().GetResult();

            Console.WriteLine("Occurence time = " + result.OccurenceTime.TotalSeconds.ToString());
            Console.WriteLine(result.Analysis.ToString());

        }
    }
}
