using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;


namespace AzureServices
{
    public static class ComputerVisionManager
    {
        const string subscriptionKey = "a75c5277e4c8441cb56b0b4762a05ba1";

        const string uriBase =
            "https://westcentralus.api.cognitive.microsoft.com/vision/v2.0";


        /// <summary>
        ///  Gets the analysis of the specified image (by path) by using
        /// the Computer Vision REST API.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static async Task<JToken> AnalyzeImage(string path)
        {
            byte[] byteData = ImageManager.GetImageBytesFromPath(path);

            return await AnalyzeImage(byteData);
        }


        /// <summary>
        /// Gets the analysis of the specified image by using
        /// the Computer Vision REST API.
        /// </summary>
        /// <param name="byteData">The byte data of image file.</param>
        public static async Task<JToken> AnalyzeImage(byte[] byteData)
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
        public static async Task<JToken> ReadHandwrittenText(string path)
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
        public static async Task<JToken> ReadHandwrittenText(byte[] byteData)
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
        public static async Task<JToken> MakeOCRRequest(string path)
        {
            byte[] byteData = ImageManager.GetImageBytesFromPath(path);

            return await MakeOCRRequest(byteData);
        }


        /// <summary>
        /// Gets the text visible in the specified image file by using
        /// the Computer Vision REST API.
        /// </summary>
        /// <param name="imageFilePath">The image file with printed text.</param>
        public static async Task<JToken> MakeOCRRequest(byte[] byteData)
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
    }
}
