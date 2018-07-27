using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using templibrary;

namespace HelperApiNetFramework.Controllers
{
    public class VideoManagerController : ApiController
    {
        VideoManager Manager = new VideoManager();

        public struct PATH
        {
            public string Path { get; set; }
        }

        // POST: api/VideoManager
        public IHttpActionResult Post([FromBody]PATH path)
        {
            if (path.Equals(null))
            {
                return BadRequest();
            }

            VideoManager.testingVideoReaderONLY();

            return Ok();
        }
        
    }
}
