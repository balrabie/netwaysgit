using Accord.Video.FFMPEG;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureServices
{
    public class VideoManager
    {
        /// <summary>
        /// delete later
        /// </summary>
        public static void testingVideoReaderONLY()
        {
            Console.WriteLine("testingVideoReaderONLY START");

            const string path = @"D:\Users\bahid\Desktop\myPrograms\437 Project\videos\vid2.mp4";

            VideoFileReader reader = new VideoFileReader();

            reader.Open(path);

            Console.WriteLine("testingVideoReaderONLY processing...");

            reader.Close();

            Console.WriteLine("testingVideoReaderONLY END");
        }
        /// <summary>
        /// Checks the video for matches.
        /// </summary>
        /// <param name="videoPath">The video path.</param>
        /// <param name="desiredTag">The desired tag to look for in the video.</param>
        /// <param name="startingPoint">The starting point of the search (in seconds).</param>
        /// <returns> Image data object </returns>
        public static async Task<ImageDto> CheckVideoForMatches
            (string videoPath, string desiredTag, double startingPoint = 0)
        {
            VideoFileReader reader = new VideoFileReader();

            reader.Open(videoPath);

            double rate = reader.FrameRate.ToDouble();

            for (int i = 0; i < reader.FrameCount; i++)
            {
                try
                {
                    byte[] frame = ImageManager.ToBytes(reader.ReadVideoFrame());

                    double current_seconds = i * 1.0 / rate;

                    if (i % rate != 0 || startingPoint > current_seconds)
                    // for efficiency: only do analysis every 1 second (or every N=rate frames)
                    {
                        continue;
                    }

                    JToken analysis = await ComputerVisionManager.AnalyzeImage(frame);

                    if (ImageManager.CheckMatch(desiredTag, analysis))
                    {
                        reader.Close();
                        
                        return new ImageDto()
                        {    
                            Analysis = analysis,

                            OccurenceTime = 
                            TimeSpan.FromSeconds(current_seconds),

                            DesiredTag = desiredTag
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

