using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AzureServices
{
    public class TextLanguageManager
    {
        private static string host = "https://api.cognitive.microsofttranslator.com";

        private static string key = "f29218f76b10455ca4eb0582432080b2";


        /// <summary>
        /// Detects the language of the input text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static async Task<JToken> DetectLanguage(string text)
        {
            string path = "/detect?api-version=3.0";
            string uri = host + path;
            System.Object[] body = new System.Object[] { new { Text = text } };
            var requestBody = JsonConvert.SerializeObject(body);

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(uri);
                request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                request.Headers.Add("Ocp-Apim-Subscription-Key", key);

                var response = await client.SendAsync(request);
                var responseBody = await response.Content.ReadAsStringAsync();
                var result = JsonConvert
                    .SerializeObject(JsonConvert.DeserializeObject(responseBody), Formatting.Indented);

                return JToken.Parse(result);
            }
            
        }


        /// <summary>
        /// Auto detects input text language, then translates to desired languages.
        /// </summary>
        /// <param name="text">The input text.</param>
        /// <param name="languages">The desired languages.</param>
        /// <returns></returns>
        public static async Task<JToken> Translate(string text, params string[] languages)
        {
            string path = "/translate?api-version=3.0";

            StringBuilder stringBuilder = new StringBuilder();

            foreach (string language in languages)
            {
                stringBuilder.Append($"&to={language}");
            }
            string params_ = stringBuilder.ToString();

            string uri = host + path + params_;

            System.Object[] body = new System.Object[] { new { Text = text } };
            var requestBody = JsonConvert.SerializeObject(body);

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri(uri);
                request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                request.Headers.Add("Ocp-Apim-Subscription-Key", key);

                var response = await client.SendAsync(request);
                var responseBody = await response.Content.ReadAsStringAsync();
                var result = JsonConvert
                    .SerializeObject(JsonConvert.DeserializeObject(responseBody), Formatting.Indented);

                return JToken.Parse(result);
            }
        }
    }
}
