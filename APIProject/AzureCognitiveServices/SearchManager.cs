﻿using AzureCognitiveServices.Models_img;
using AzureCognitiveServices.Models_web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AzureCognitiveServices
{
    public class SearchManager
    {
        public struct SearchResult
        {
            public JToken jsonResult;
            public Dictionary<String, String> relevantHeaders;
        }

        public async Task<WebSearchDto> GetBingWebSearch(string searchQuery)
        {
            JToken json = (await BingWebSearch(searchQuery)).jsonResult;

            WebSearchDto webSearchDto 
                = JsonConvert.DeserializeObject<WebSearchDto>(json.ToString());

            return webSearchDto;
        }

        public async Task<ImageSearchDto> GetBingImageSearch(string searchQuery)
        {
            JToken json = (await BingWebSearch(searchQuery)).jsonResult;

            ImageSearchDto imageSearchDto 
                = JsonConvert.DeserializeObject<ImageSearchDto>(json.ToString());

            return imageSearchDto;
        }

        /// <summary>
        /// Performs a Bing Web search, and returns the result as SearchResult
        /// </summary>
        /// <param name="searchQuery">The search query.</param>
        /// <returns></returns>
        private static async Task<SearchResult> BingWebSearch(string searchQuery)
        {
            // Construct the URI of the search request
            var uriQuery = uriBase + "?q=" + Uri.EscapeDataString(searchQuery);

            // Perform the Web request and get the response
            WebRequest request = HttpWebRequest.Create(uriQuery);
            request.Headers["Ocp-Apim-Subscription-Key"] = accessKey;

            HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();


            //HttpWebResponse response = (HttpWebResponse)request.GetResponseAsync().Result;
            string contentString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            // Create result object for return
            var searchResult = new SearchResult()
            {
                jsonResult = JToken.Parse(contentString),
                relevantHeaders = new Dictionary<String, String>()
            };

            // Extract Bing HTTP headers
            foreach (String header in response.Headers)
            {
                if (header.StartsWith("BingAPIs-") || header.StartsWith("X-MSEdge-"))
                    searchResult.relevantHeaders[header] = response.Headers[header];
            }

            return searchResult;
        }

        /// <summary>
        /// Performs a Bing Image search, and returns the results as a SearchResult.
        /// </summary>
        /// <param name="searchQuery">The search query.</param>
        /// <returns></returns>
        private static async Task<SearchResult> BingImageSearch(string searchQuery)
        {
            // Construct the URI of the search request
            var uriQuery = uriBase_img + "?q=" + Uri.EscapeDataString(searchQuery);

            // Perform the Web request and get the response
            WebRequest request = HttpWebRequest.Create(uriQuery);
            request.Headers["Ocp-Apim-Subscription-Key"] = accessKey;
            HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
            string json = new StreamReader(response.GetResponseStream()).ReadToEnd();

            // Create result object for return
            var searchResult = new SearchResult()
            {
                jsonResult = json,
                relevantHeaders = new Dictionary<String, String>()
            };

            // Extract Bing HTTP headers
            foreach (String header in response.Headers)
            {
                if (header.StartsWith("BingAPIs-") || header.StartsWith("X-MSEdge-"))
                    searchResult.relevantHeaders[header] = response.Headers[header];
            }

            return searchResult;
        }

        private const string accessKey = "f6a131581ecf4784bcfdb19cef2a3256";

        private const string uriBase = "https://api.cognitive.microsoft.com/bing/v7.0/search";
        private const string uriBase_img = "https://api.cognitive.microsoft.com/bing/v7.0/images/search";
    }
}