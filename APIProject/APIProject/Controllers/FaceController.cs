using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureCognitiveServices;
using AzureCognitiveServices.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace APIProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FaceController : ControllerBase
    {
        FaceManager Manager = new FaceManager();

        
        [HttpPost("Analysis")]
        public async Task<ActionResult<List<FaceDto>>> AnalyzeFace([FromBody] Path path)
        {
            if (path.TargetImage == null)
            {
                return BadRequest();
            }
            
            var result = await Manager.GetFaceDetection(path.TargetImage);

            return Ok(result);
        }
    }
}