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
    public class TranslationController : ControllerBase
    {
        TranslationManager Manager = new TranslationManager();

        // GET: api/Translation/DetectLanguage/{text}
        [HttpGet("DetectLanguage/{text}")]
        public async Task<ActionResult<string>> GetLanguage([FromRoute]string text)
        {
            if (text == null || text == string.Empty)
            {
                return BadRequest();
            }

            Manager.OriginalText = text;

            var result = await Manager.GetDetectedLanguage();

            return Ok(result);
        }


        // Post: api/Translation
        [HttpPost("{text}")]
        public async Task<ActionResult<TranslationDto>> 
            GetLanguage([FromRoute]string text, [FromBody] string[] languages)
        {
            if (text == null || text == string.Empty || languages == null)
            {
                return BadRequest();
            }

            Manager.OriginalText = text;

            var result = await Manager.GetTranslation();

            return Ok(result);
        }

    }
}
