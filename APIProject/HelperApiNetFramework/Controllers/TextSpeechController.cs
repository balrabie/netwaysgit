﻿using System;
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
    public class TextSpeechController : ApiController
    {
        private SpeechToTextManager SpeechToTextManager = new SpeechToTextManager();
        private TextToSpeechManager TextToSpeechManager = new TextToSpeechManager();

        public struct AudioFilePath
        {
            public string Path { get; set; }
        }

        [Route("SpeechToText")]
        [HttpPost]
        public async Task<IHttpActionResult> ToText([FromBody] AudioFilePath audioFilePath)
        {
            if (audioFilePath.Path == null)
            {
                return BadRequest("Null query");
            }

            try
            {
                var result = await SpeechToTextManager.RecognizeSpeechAsync(audioFilePath.Path);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [Route("TextToSpeech/{text}")]
        [HttpPost]
        public async Task<IHttpActionResult> ToSpeech(string text)
        {
            if (text == null)
            {
                return BadRequest("Null query");
            }
            try
            {
                var result = await TextToSpeechManager.GenerateSpeechWithDefaultSettings(text);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
