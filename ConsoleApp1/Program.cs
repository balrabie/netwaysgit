using Accord.Video.FFMPEG;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static tester.ImageManager;

namespace tester
{

    class Program
    {

        const string img_path = @"D:\Users\bahid\Desktop\Images\house.png";

        const string vid_path = @"D:\Users\bahid\Desktop\myPrograms\437 Project\videos\london.mp4";

        static void Main(string[] args)
        {
            ImageDto image = CheckVideoForMatches(vid_path, "people", 12).GetAwaiter().GetResult();

            Console.WriteLine($"tag : {image.Title}");
            Console.WriteLine($"occurence at : {image.OccurenceTime}");

            Bitmap bitmap = ImageManager.ByteToImage(image.Bytes);
            bitmap.Save(@"D:\Users\bahid\Desktop\match.Jpeg", ImageFormat.Jpeg);
        }

        /// <summary>
        /// Checks the video for matches.
        /// </summary>
        /// <param name="videoPath">The video path.</param>
        /// <param name="desiredTag">The desired tag to look for in the video.</param>
        /// <param name="startingPoint">The starting point of the search (in seconds).</param>
        /// <returns> Image data object </returns>
        private static async Task<ImageDto> CheckVideoForMatches
            (string videoPath, string desiredTag, double startingPoint = 0)
        {
            VideoFileReader reader = new VideoFileReader();

            reader.Open(videoPath);

            double rate = reader.FrameRate.ToDouble();

            for (int i = 0; i < reader.FrameCount; i++)
            {
                try
                {
                    byte[] frame = ImageManager.ImageToByte(reader.ReadVideoFrame());

                    double current_seconds = i * 1.0 / rate;

                    if (i % rate != 0 || startingPoint > current_seconds)
                    // for efficiency: only do analysis every 1 second (or every N=rate frames)
                    {
                        continue;
                    }

                    JToken analysis = await ComputerVisionManager.MakeAnalysisRequest(frame);

                    if (ImageManager.CheckMatch(desiredTag, analysis))
                    {
                        reader.Close();

                        return new ImageDto()
                        {
                            Title = desiredTag,

                            OccurenceTime =
                            TimeSpan.FromSeconds(current_seconds).ToString(@"hh\:mm\:ss\:fff"),

                            Bytes = frame
                        };

                    }

                }
                catch (Exception)
                {
                    continue;
                }
            }
            reader.Close();
            return null;
        }
    }
}
