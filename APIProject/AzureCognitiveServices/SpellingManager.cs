﻿using AzureCognitiveServices.Models;
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
    public class SpellingManager
    {
        public string Text { get; set; }


        public async Task<List<SpellingDto>> GetSuggestion()
        {
            JToken jToken = await SpellCheck(Text);

            return ReadFromJson(jToken);
        }


        private List<SpellingDto> ReadFromJson(JToken jToken)
        {
            List<SpellingDto> spellingDtos = new List<SpellingDto>();

            JArray flaggedTokens = (JArray)jToken["flaggedTokens"];

            foreach (JToken flaggedToken in flaggedTokens)
            {
                string word = flaggedToken["token"].ToString();
                List<string> suggestions = new List<string>();

                foreach (JToken suggestionToken in (JArray)flaggedToken["suggestions"])
                {
                    suggestions.Add(suggestionToken["suggestion"].ToString());
                }

                spellingDtos.Add(new SpellingDto()
                {
                    Word = word,
                    Suggestions = suggestions
                });
            }

            return spellingDtos;
        }


        /// <summary>
        /// Checks the spelling/grammar of the given text
        /// </summary>
        /// <param name="text">The text to be checked.</param>
        /// <returns></returns>
        private static async Task<JToken> SpellCheck(string text)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", key);


            HttpResponseMessage response = new HttpResponseMessage();
            string uri = host + path + params_;

            List<KeyValuePair<string, string>> values = new List<KeyValuePair<string, string>>();
            values.Add(new KeyValuePair<string, string>("text", text));

            using (FormUrlEncodedContent content = new FormUrlEncodedContent(values))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                response = await client.PostAsync(uri, content);
            }

            string client_id;
            if (response.Headers.TryGetValues("X-MSEdge-ClientID", out IEnumerable<string> header_values))
            {
                client_id = header_values.First();
            }

            string contentString = await response.Content.ReadAsStringAsync();

            return JToken.Parse(contentString);

        }

        private static string host = "https://api.cognitive.microsoft.com";

        private static string path = "/bing/v7.0/spellcheck?";

        // For a list of available markets, go to:
        // https://docs.microsoft.com/rest/api/cognitiveservices/bing-autosuggest-api-v7-reference#market-codes
        private static string params_ = "mkt=en-US&mode=proof"; //**********************

        private static string key = "bacce7d22b004f63a90ce2fe045f7e0c";

    }

}
