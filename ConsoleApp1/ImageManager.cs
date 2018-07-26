using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tester
{
    public class ImageManager
    {
        public class ImageDto
        {
            public string Title { get; set; }

            public byte[] Bytes { get; set; }

            public string OccurenceTime { get; set; }
        }

        public static byte[] GetImageAsByteArray(string imageFilePath)
        {
            using (FileStream fileStream =
                new FileStream(imageFilePath, FileMode.Open, FileAccess.Read))
            {
                BinaryReader binaryReader = new BinaryReader(fileStream);
                return binaryReader.ReadBytes((int)fileStream.Length);
            }
        }

        public static byte[] ImageToByte(Image image)
        {
            using (var stream = new MemoryStream())
            {
                image.Save(stream, ImageFormat.Png);
                return stream.ToArray();
            }
        }

        public static Bitmap ByteToImage(byte[] imageData)
        {
            Bitmap bmp;
            using (var ms = new MemoryStream(imageData))
            {
                bmp = new Bitmap(ms);
            }
            return bmp;
        }

        public static JToken[] GetTags(JToken jToken)
        {
            return jToken["description"]["tags"].ToArray();
        }

        public static bool CheckMatch(string desiredTag, JToken jToken)
        {
            JToken[] imageTags = GetTags(jToken);

            foreach (JToken tag in imageTags)
            {
                if (desiredTag == tag.ToString())
                {
                    return true;
                }
            }

            return false;
        }
    }
}
