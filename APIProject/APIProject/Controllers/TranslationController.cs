using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AzureCognitiveServices;
using AzureCognitiveServices.Models;

namespace APIProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TextController : ControllerBase
    {
        TranslationManager TranslationManager = new TranslationManager();

        SpellingManager SpellingManager = new SpellingManager();


        // GET: api/Text/DetectLanguage/Text
        [HttpGet("DetectLanguage/{text}")]
        public async Task<ActionResult<string>> GetLanguage([FromRoute]string text)
        {
            if (text == null || text == string.Empty)
            {
                return BadRequest();
            }

            TranslationManager.OriginalText = text;

            var result = await TranslationManager.GetDetectedLanguage();

            return Ok(result);
        }


        // Post: api/Text (acts as GET)
        [HttpPost("{text}")]
        public async Task<ActionResult<TranslationDto>> 
            GetLanguage([FromRoute]string text, [FromBody] string[] languages)
        {
            if (text == null || text == string.Empty || languages == null)
            {
                return BadRequest();
            }

            TranslationManager.OriginalText = text;

            var result = await TranslationManager.GetTranslation(languages);

            return Ok(result);
        }


        // Post: api/CheckSpelling/Text (acts as GET)
        [HttpGet("CheckSpelling/{text}")]
        public async Task<ActionResult<SpellingDto>> CheckSpelling([FromRoute]string text)
        {
            if (text == null || text == string.Empty)
            {
                return BadRequest();
            }

            SpellingManager.Text = text;

            var result = await SpellingManager.GetSuggestion();

            return Ok(result);
        }

    }
}
