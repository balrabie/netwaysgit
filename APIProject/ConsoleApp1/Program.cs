using System;
using AzureCognitiveServices;

namespace ConsoleApp1
{
    class Program
    {
        const string PATH = "D:\\Users\\bahid\\Desktop\\Images\\handwriting.jpg";
        const string PATH_2 = "D:\\Users\\bahid\\Desktop\\Images\\harry-meghan-15.jpg";


        static void Main(string[] args)
        {

            FaceManager fm = new FaceManager();

            var result = fm.GetFaceDetection(PATH_2)
                .GetAwaiter().GetResult();
        }
    }
}
