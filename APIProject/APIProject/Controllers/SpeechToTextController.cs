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
    public class SpeechToTextController : ControllerBase
    {
        private SpeechToTextManager speechToTextManager = new SpeechToTextManager();

        [HttpPost("SpeechToText")]
        public async Task<ActionResult<SpeechToTextDto>> ToText([FromBody] Path audioFilePath)
        {
            if (audioFilePath.Target == null)
            {
                return BadRequest("Null query");
            }

            try
            {
                var result = await speechToTextManager.RecognizeSpeechAsync(audioFilePath.Target);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}