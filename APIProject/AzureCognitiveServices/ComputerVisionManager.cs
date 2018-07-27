using AzureCognitiveServices.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AzureCognitiveServices
{
    public class ComputerVisionManager
    {
        /// <summary>
        /// Gets the image analysis. Returns Image Dto
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public async Task<ImageAnalysisDto> GetImageAnalysis(string path)
        {
            JToken json = await AnalyzeImage(path);

            List<Category> categories = new List<Category>();
            List<string> tags = new List<string>();
            List<string> comments = new List<string>();
            //string comment = json["description"]["captions"]["text"].ToString(); //**

            foreach (JToken comment in (JArray)json["description"]["captions"])
            {
                comments.Add(comment["text"].ToString());
            }

            List<string> colors = new List<string>();

            foreach (JToken category in (JArray)json["categories"])
            {
                categories.Add(
                    new Category()
                    {
                        Name = category["name"].ToString(),
                        Confidence = Convert.ToDouble(category["score"].ToString())
                    });
            }

            foreach (JToken tag in (JArray)(json["description"]["tags"]))
            {
                tags.Add(tag.ToString());
            }

            foreach (JToken color in (JArray)json["color"]["dominantColors"])
            {
                colors.Add(color.ToString());
            }

            return new ImageAnalysisDto()
            {
                Categories = categories,
                Tags = tags,
                Comments = comments,
                DominantColors = colors
            };
        }


        /// <summary>
        /// Gets the text of a handwritten document. Returns Handwriting Dto
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public async Task<HandwritingDto> GetHandwrittenText(string path)
        {
            JToken json = await ReadHandwrittenText(path);

            List<string> lines = new List<string>();

            Console.WriteLine(json.ToString());

            foreach (JToken line in (JArray)json["recognitionResult"]["lines"])
            {
                lines.Add(line["text"].ToString() + " ");
            }

            return new HandwritingDto()
            {
                Lines = lines
            };
        }


        /// <summary>
        /// Gets the text of a printed document. Returns OCRDto
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public async Task<OCRDto> GetOCRText(string path)
        {
            JToken json = await MakeOCRRequest(path);

            List<string> lines = new List<string>();

            foreach (JToken line in (JArray)json["regions"][0]["lines"])
            {
                StringBuilder words = new StringBuilder();

                foreach (JToken word in (JArray)line["words"])
                {
                    words.Append(word["text"].ToString() + " ");
                }

                lines.Add(words.ToString());
            }

            return new OCRDto()
            {
                Language = json["language"].ToString(),
                Lines = lines
            };
        }

        /// <summary>
        ///  Gets the analysis of the specified image (by path) by using
        /// the Computer Vision REST API.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        private static async Task<JToken> AnalyzeImage(string path)
        {
            byte[] byteData = ImageManager.GetImageBytesFromPath(path);

            return await AnalyzeImage(byteData);
        }


        /// <summary>
        /// Gets the analysis of the specified image by using
        /// the Computer Vision REST API.
        /// </summary>
        /// <param name="byteData">The byte data of image file.</param>
        private static async Task<JToken> AnalyzeImage(byte[] byteData)
        {
            try
            {
                HttpClient client = new HttpClient();

                // Request headers.
                client.DefaultRequestHeaders.Add(
                    "Ocp-Apim-Subscription-Key", subscriptionKey);

                // Request parameters. A third optional parameter is "details".
                string requestParameters =
                    "visualFeatures=Categories,Description,Color";

                // Assemble the URI for the REST API Call.
                string uri = uriBase + @"/analyze?" + requestParameters;

                HttpResponseMessage response;

                // Request body. Posts a locally stored JPEG image.


                using (ByteArrayContent content = new ByteArrayContent(byteData))
                {
                    // This example uses content type "application/octet-stream".
                    // The other content types you can use are "application/json"
                    // and "multipart/form-data".
                    content.Headers.ContentType =
                        new MediaTypeHeaderValue("application/octet-stream");

                    // Make the REST API call.
                    response = await client.PostAsync(uri, content);
                }

                // Get the JSON response.
                string contentString = await response.Content.ReadAsStringAsync();

                // Display the JSON response.
                return JToken.Parse(contentString);
            }
            catch (Exception e)
            {
                Console.WriteLine("\n" + e.Message);
                return null;
            }
        }


        /// <summary>
        /// Gets the handwritten text from the specified image file (by path) by using
        /// the Computer Vision REST API.
        /// </summary>
        /// <param name="byteData">The byte data.</param>
        /// <returns></returns>
        private static async Task<JToken> ReadHandwrittenText(string path)
        {
            byte[] byteData = ImageManager.GetImageBytesFromPath(path);

            return await ReadHandwrittenText(byteData);
        }


        /// <summary>
        /// Gets the handwritten text from the specified image file by using
        /// the Computer Vision REST API.
        /// </summary>
        /// <param name="byteData">The byte data.</param>
        /// <returns></returns>
        private static async Task<JToken> ReadHandwrittenText(byte[] byteData)
        {
            try
            {
                HttpClient client = new HttpClient();

                // Request headers.
                client.DefaultRequestHeaders.Add(
                    "Ocp-Apim-Subscription-Key", subscriptionKey);

                // Request parameter.
                // Note: The request parameter changed for APIv2.
                // For APIv1, it is "handwriting=true".
                string requestParameters = "mode=Handwritten";

                // Assemble the URI for the REST API Call.
                string uri = uriBase + @"/recognizeText?" + requestParameters;

                HttpResponseMessage response;

                // Two REST API calls are required to extract handwritten text.
                // One call to submit the image for processing, the other call
                // to retrieve the text found in the image.
                // operationLocation stores the REST API location to call to
                // retrieve the text.
                string operationLocation;

                using (ByteArrayContent content = new ByteArrayContent(byteData))
                {
                    // This example uses content type "application/octet-stream".
                    // The other content types you can use are "application/json"
                    // and "multipart/form-data".
                    content.Headers.ContentType =
                        new MediaTypeHeaderValue("application/octet-stream");

                    // The first REST call starts the async process to analyze the
                    // written text in the image.
                    response = await client.PostAsync(uri, content);
                }

                // The response contains the URI to retrieve the result of the process.
                if (response.IsSuccessStatusCode)
                {
                    operationLocation =
                          response.Headers.GetValues("Operation-Location").FirstOrDefault();
                }
                else
                {
                    // Display the JSON error data.
                    string errorString = await response.Content.ReadAsStringAsync();
                    throw new Exception(JToken.Parse(errorString).ToString());
                }

                // The second REST call retrieves the text written in the image.
                //
                // Note: The response may not be immediately available. Handwriting
                // recognition is an async operation that can take a variable amount
                // of time depending on the length of the handwritten text. You may
                // need to wait or retry this operation.
                //
                // This example checks once per second for ten seconds.
                string contentString;
                int i = 0;
                do
                {
                    System.Threading.Thread.Sleep(1000);
                    response = await client.GetAsync(operationLocation);
                    contentString = await response.Content.ReadAsStringAsync();
                    ++i;
                }
                while (i < 10 && contentString.IndexOf("\"status\":\"Succeeded\"") == -1);

                if (i == 10 && contentString.IndexOf("\"status\":\"Succeeded\"") == -1)
                {
                    throw new Exception("\nTimeout error.\n");
                }

                // return the JSON response.
                return JToken.Parse(contentString);

            }
            catch (Exception e)
            {
                Console.WriteLine("\n" + e.Message);
                return null;
            }
        }


        /// <summary>
        /// Gets the text visible in the specified image file (by path) by using
        /// the Computer Vision REST API.
        /// </summary>
        /// <param name="imageFilePath">The image file with printed text.</param>
        private static async Task<JToken> MakeOCRRequest(string path)
        {
            byte[] byteData = ImageManager.GetImageBytesFromPath(path);

            return await MakeOCRRequest(byteData);
        }


        /// <summary>
        /// Gets the text visible in the specified image file by using
        /// the Computer Vision REST API.
        /// </summary>
        /// <param name="imageFilePath">The image file with printed text.</param>
        private static async Task<JToken> MakeOCRRequest(byte[] byteData)
        {
            try
            {
                HttpClient client = new HttpClient();

                // Request headers.
                client.DefaultRequestHeaders.Add(
                    "Ocp-Apim-Subscription-Key", subscriptionKey);

                // Request parameters.
                string requestParameters = "language=unk&detectOrientation=true";

                // Assemble the URI for the REST API Call.
                string uri = uriBase + @"/ocr?" + requestParameters;

                HttpResponseMessage response;

                using (ByteArrayContent content = new ByteArrayContent(byteData))
                {
                    // This example uses content type "application/octet-stream".
                    // The other content types you can use are "application/json"
                    // and "multipart/form-data".
                    content.Headers.ContentType =
                        new MediaTypeHeaderValue("application/octet-stream");

                    // Make the REST API call.
                    response = await client.PostAsync(uri, content);
                }

                // Get the JSON response.
                string contentString = await response.Content.ReadAsStringAsync();

                // return the JSON response.
                return JToken.Parse(contentString);
            }
            catch (Exception e)
            {
                Console.WriteLine("\n" + e.Message);
                return null;
                // return null; OR (throw e;) ?? *
            }
        }


        private const string subscriptionKey = "38c72a73df1d4459bccb6eecae71daa1";

        private const string uriBase =
            "https://westcentralus.api.cognitive.microsoft.com/vision/v2.0";
    }

}
