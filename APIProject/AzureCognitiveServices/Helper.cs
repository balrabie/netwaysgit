using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureCognitiveServices
{
    public static class Helper
    {
        private const string PATH = @"D:\Users\bahid\Desktop\Output\temp";

        public static async Task WriteJsonFile(string name, string text, string path = PATH)
        {
            if (!Directory.Exists(path))
            {
                throw new DirectoryNotFoundException();
            }
            using (StreamWriter writer = File.CreateText(path + @"\" + name + ".Json"))
            {
                await writer.WriteLineAsync(text);
            }
        }
    }
}
