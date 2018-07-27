using AzureCognitiveServices.Models;
using Newtonsoft.Json;
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
    public class FaceManager
    {
        /// <summary>
        /// Analyzes detected faces.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public async Task<List<FaceDto>> GetFaceDetection(string path)
        {
            JToken json = await DetectFaces(path);

            List<FaceDto> faceDto = JsonConvert.DeserializeObject<List<FaceDto>>(json.ToString());

            return faceDto;            
        }


        /// <summary>
        /// Detects the faces in the specified image (by path) by using the Face REST API.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        private static async Task<JToken> DetectFaces(string path)
        {
            byte[] byteData = ImageManager.GetImageBytesFromPath(path);

            return await DetectFaces(byteData);
        }


        /// <summary>
        /// Detects the faces in the specified image by using the Face REST API.
        /// </summary>
        /// <param name="byteData">The byte data of image file.</param>
        private static async Task<JToken> DetectFaces(byte[] byteData)
        {
            HttpClient client = new HttpClient();

            // Request headers.
            client.DefaultRequestHeaders.Add(
                "Ocp-Apim-Subscription-Key", subscriptionKey);

            // Request parameters. A third optional parameter is "details".
            string requestParameters = "returnFaceId=true&returnFaceLandmarks=false" +
                "&returnFaceAttributes=age,gender,headPose,smile,facialHair,glasses," +
                "emotion,hair,makeup,occlusion,accessories,blur,exposure,noise";

            // Assemble the URI for the REST API Call.
            string uri = uriBase + "?" + requestParameters;

            HttpResponseMessage response;

            using (ByteArrayContent content = new ByteArrayContent(byteData))
            {
                // This example uses content type "application/octet-stream".
                // The other content types you can use are "application/json"
                // and "multipart/form-data".
                content.Headers.ContentType =
                    new MediaTypeHeaderValue("application/octet-stream");

                // Execute the REST API call.
                response = await client.PostAsync(uri, content);

                // Get the JSON response.
                string contentString = await response.Content.ReadAsStringAsync();

                // Display the JSON response.
                return JToken.Parse(contentString);
            }
        }



        private const string subscriptionKey = "20696cb8f6a043ceafe82a9c113b2c7f";

        private const string uriBase =
            "https://westcentralus.api.cognitive.microsoft.com/face/v1.0/detect";
    }
}
