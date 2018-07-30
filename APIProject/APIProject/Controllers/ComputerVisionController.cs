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
    public class ComputerVisionController : ControllerBase
    {
        ComputerVisionManager Manager = new ComputerVisionManager();

        // Post: api/Text 
        [HttpPost("OCR")]
        public async Task<ActionResult<OCRDto>> GetPrintedText([FromBody] Path path)
        {
            if (path.Target == null || path.Target == string.Empty)
            {
                return BadRequest("Path is not valid");
            }

            try
            {
                OCRDto result = await Manager.GetOCRText(path.Target);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
                       
        }


        [HttpPost("Handwritten")]
        public async Task<ActionResult<HandwritingDto>> GetHandwriting([FromBody] Path path)
        {
            if (path.Target == null || path.Target == string.Empty)
            {
                return BadRequest("Path is not valid");
            }

            try
            {
                HandwritingDto result = await Manager.GetHandwrittenText(path.Target);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Analysis")]
        public async Task<ActionResult<ImageAnalysisDto>> GetImageAnalysis([FromBody] Path path)
        {
            if (path.Target == null || path.Target == string.Empty)
            {
                return BadRequest("Path is not valid");
            }

            try
            {
                ImageAnalysisDto result = await Manager.GetImageAnalysis(path.Target);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}