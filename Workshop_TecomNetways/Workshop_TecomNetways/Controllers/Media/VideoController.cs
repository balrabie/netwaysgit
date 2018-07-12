using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Workshop_TecomNetways.DTO;
using Workshop_TecomNetways.Models;
using Workshop_TecomNetways.Repository;

namespace Workshop_TecomNetways.Controllers
{

    public class VideoController : ApiController
    {
        private UnitOfWork UoW = null;
        private IMapper EntityToDtoIMapper = null;
        private IMapper DtoToEntityIMapper = null;

        public VideoController()
        {
            UoW = new UnitOfWork();
            InitializeMapping();

        }

        public VideoController(UnitOfWork uoW)
        {
            UoW = uoW;
            InitializeMapping();
        }

        private void InitializeMapping()
        {
            EntityToDtoIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<Video, VideoDto>())
                .CreateMapper();
            DtoToEntityIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<VideoDto, Video>())
                .CreateMapper();
        }

        // GET: api/Videos
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            var items = UoW.GetRepository<Video>().GetAll().ToList();
            var DTO = EntityToDtoIMapper.Map<List<Video>, List<VideoDto>>(items).ToList();

            return Ok(DTO);
        }

        // GET: api/Videos/5
        [ResponseType(typeof(Video))]
        public async Task<IHttpActionResult> GetVideo(int id)
        {
            Video item = await UoW.GetRepository<Video>().GetItemAsycn(e => e.ID == id);
            if (item == null)
            {
                return NotFound();
            }

            var DTO = EntityToDtoIMapper.Map<Video, VideoDto>(item);
            return Ok(DTO);
        }

        // PUT: api/Videos/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutVideo(int id, VideoDto videoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != videoDto.ID)
            {
                return BadRequest();
            }
            var video = DtoToEntityIMapper.Map<VideoDto, Video>(videoDto); ////
            UoW.GetRepository<Video>().ModifyEntityState(video);

            try
            {
                await UoW.SaveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VideoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Videos
        [ResponseType(typeof(Video))]
        public async Task<IHttpActionResult> PostVideo(VideoDto videoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var video = DtoToEntityIMapper.Map<VideoDto, Video>(videoDto); ////
            UoW.GetRepository<Video>().Insert(video);
            await UoW.SaveAsync();

            return CreatedAtRoute("DefaultApi", new { id = video.ID }, video);
        }

        // DELETE: api/Videos/5
        [ResponseType(typeof(Video))]
        public async Task<IHttpActionResult> DeleteVideo(int id)
        {
            Video video = await UoW.GetRepository<Video>().GetItemAsycn(e => e.ID == id);
            if (video == null)
            {
                return NotFound();
            }

            UoW.GetRepository<Video>().Delete(video);
            await UoW.SaveAsync();

            return Ok(video);
        }



        private bool VideoExists(int id)
        {
            return UoW.GetRepository<Video>().GetItem(e => e.ID == id) != null;
        }
    }
}
