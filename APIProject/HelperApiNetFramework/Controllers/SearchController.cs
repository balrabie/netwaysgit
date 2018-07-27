using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AzureCognitiveServices;
using AzureCognitiveServices.Models;


namespace HelperApiNetFramework.Controllers
{
    public class SearchController : ApiController
    {
        SearchManager Manager = new SearchManager();

        [Route("WebSearch/{query}")]
        [HttpPost]
        public async Task<IHttpActionResult> WebSearch(string query)
        {
            if (query == null)
            {
                return BadRequest("Null query");
            }

            var result = await Manager.GetBingWebSearch(query);

            return Ok(result);
        }


    }
}
