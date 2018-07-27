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
        [HttpPost("LanguageDetection")]
        public async Task<ActionResult<string>> GetLanguage([FromBody] Input input)
        {
            if (input.Text == null)
            {
                return BadRequest();
            }

            TranslationManager.OriginalText = input.Text;

            var result = await TranslationManager.GetDetectedLanguage();

            return Ok(result);
        }


        // Post: api/Text 
        [HttpPost("Translation")]
        public async Task<ActionResult<TranslationDto>> 
            TranslateText([FromBody] TranslationInput translationInput)
        {
            if (translationInput.Text == null  || translationInput.To == null)
            {
                return BadRequest();
            }

            TranslationManager.OriginalText = translationInput.Text;

            var result = await TranslationManager.GetTranslation(translationInput.To);

            return Ok(result);
        }


        // Post: api/CheckSpelling/Text 
        [HttpPost("CheckSpelling")]
        public async Task<ActionResult<SpellingDto>> CheckSpelling([FromBody] Input input)
        {
            if (input.Text == null)
            {
                return BadRequest();
            }

            SpellingManager.Text = input.Text;

            var result = await SpellingManager.GetSuggestion();

            return Ok(result);
        }

    }
}
