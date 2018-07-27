﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureCognitiveServices
{
    public class ImageManager
    {
        /// <summary>
        /// Converts Images(Bitmap) to byte array.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <returns></returns>
        public static byte[] ToBytes(Image image)
        {
            byte[] bytes;
            using (var stream = new MemoryStream())
            {
                image.Save(stream, ImageFormat.Png);
                bytes = stream.ToArray();
            }
            return bytes;
        }

        /// <summary>
        /// Converts a byte array to image(Bitmap).
        /// </summary>
        /// <param name="bytes">The image data.</param>
        /// <returns></returns>
        public static Bitmap BytesToImage(byte[] bytes)
        {
            Bitmap bitmap;
            using (var ms = new MemoryStream(bytes))
            {
                bitmap = new Bitmap(ms);
            }
            return bitmap;
        }

        /// <summary>
        /// Returns the contents of the specified file as a byte array.
        /// </summary>
        /// <param name="imageFilePath">The image file to read.</param>
        /// <returns>The byte array of the image data.</returns>
        public static byte[] GetImageBytesFromPath(string imageFilePath)
        {
            using (FileStream fileStream =
                new FileStream(imageFilePath, FileMode.Open, FileAccess.Read))
            {
                BinaryReader binaryReader = new BinaryReader(fileStream);
                return binaryReader.ReadBytes((int)fileStream.Length);
            }
        }

        /// <summary>
        /// MAYBE WILL DELETE LATER****
        /// Given an JToken generated by Computervision analysis:
        /// returns description tags of the image
        /// </summary>
        /// <param name="jToken">The j token.</param>
        /// <returns></returns>
        public static JToken[] GetTags(JToken jToken)
        {
            return jToken["description"]["tags"].ToArray();
        }

        /// <summary>
        /// Checks for matching between desired and generated tags from the given image.
        /// </summary>
        /// <param name="desiredTag">The desired tag.</param>
        /// <param name="jToken">The j token.</param>
        /// <returns></returns>
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
