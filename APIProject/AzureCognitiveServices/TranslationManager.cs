using AzureCognitiveServices.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AzureCognitiveServices
{

    /// <summary>
    /// Contains:
    ///   1. Language detector
    ///   2. Translator
    /// Reference for language codes:
    /// https://docs.microsoft.com/en-us/azure/cognitive-services/translator/languages
    /// </summary>
    public class TranslationManager
    {
        public string OriginalText { get; set; }


        /// <summary>
        /// Gets the detected language of the input text
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetDetectedLanguage()
        {
            if (OriginalText == string.Empty)
            {
                return string.Empty;
            }

            JToken json = await DetectLanguage(OriginalText);

            return ExtractDetectedLanguage(json);
        }


        /// <summary>
        /// Gets the translation of the input text
        /// </summary>
        /// <param name="languages">The languages.</param>
        /// <returns></returns>
        public async Task<TranslationDto> GetTranslation(List<string> languages)
        {
            if (OriginalText == string.Empty)
            {
                return new TranslationDto();
            }

            var jToken = await Translate(OriginalText,languages);
  

            return new TranslationDto()
            {
                FromLanguage = (string)(jToken[0]["detectedLanguage"]["language"]),
                ToLanguage = languages,
                OriginalText = this.OriginalText,
                TranslatedText = ExtractTranslationText(jToken)
            };
        }


        /// <summary>
        /// Detects the language of the input text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        private static async Task<JToken> DetectLanguage(string text)
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
        private async Task<JToken> Translate(string text, List<string> languages)
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


        private string ExtractDetectedLanguage(JToken jToken)
        {
            return (string)(jToken[0]["language"]);
        }


        private List<string> ExtractTranslationText(JToken jToken)
        {
            var translations = (JArray)(jToken[0]["translations"]);
            List<string> result = new List<string>();

            foreach (JToken translation in translations)
            {
                result.Add((string)translation["text"]);
            }

            return result;
        }


        private static string host = "https://api.cognitive.microsofttranslator.com";

        private static string key = "f29218f76b10455ca4eb0582432080b2";
    }


}
