using AzureServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace dotNetFrameworkAPI.Controllers
{
    public class TranslationController : ApiController
    {
        TextLanguageManager Manager = new TextLanguageManager();

        [Route("Translation/{text}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetLanguage(string text)
        {
            Manager.OriginalText = text;

            string language = await Manager.GetDetectedLanguage();

            return Ok(language);
        }

        /*
        [HttpGet]
        public IHttpActionResult GetTranslation(string text, params string[] toLanguages)
        {
            return null;
        }
        */

    }
}
