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

    public class VideoAlbumController : ApiController
    {
        private UnitOfWork UoW = null;
        private IMapper EntityToDtoIMapper = null;
        private IMapper DtoToEntityIMapper = null;

        public VideoAlbumController()
        {
            UoW = new UnitOfWork();
            InitializeMapping();

        }

        public VideoAlbumController(UnitOfWork uoW)
        {
            UoW = uoW;
            InitializeMapping();
        }

        private void InitializeMapping()
        {
            EntityToDtoIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<VideoAlbum, VideoAlbumDto>())
                .CreateMapper();
            DtoToEntityIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<VideoAlbumDto, VideoAlbum>())
                .CreateMapper();
        }

        // GET: api/VideoAlbums
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            var items = UoW.GetRepository<VideoAlbum>().GetAll().ToList();
            var DTO = EntityToDtoIMapper.Map<List<VideoAlbum>, List<VideoAlbumDto>>(items).ToList();

            return Ok(DTO);
        }

        // GET: api/VideoAlbums/5
        [ResponseType(typeof(VideoAlbum))]
        public async Task<IHttpActionResult> GetVideoAlbum(int id)
        {
            VideoAlbum item = await UoW.GetRepository<VideoAlbum>().GetItemAsycn(e => e.ID == id);
            if (item == null)
            {
                return NotFound();
            }

            var DTO = EntityToDtoIMapper.Map<VideoAlbum, VideoAlbumDto>(item);
            return Ok(DTO);
        }

        // PUT: api/VideoAlbums/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutVideoAlbum(int id, VideoAlbumDto videoAlbumDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != videoAlbumDto.ID)
            {
                return BadRequest();
            }
            var videoAlbum = DtoToEntityIMapper.Map<VideoAlbumDto, VideoAlbum>(videoAlbumDto); ////
            UoW.GetRepository<VideoAlbum>().ModifyEntityState(videoAlbum);

            try
            {
                await UoW.SaveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VideoAlbumExists(id))
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

        // POST: api/VideoAlbums
        [ResponseType(typeof(VideoAlbum))]
        public async Task<IHttpActionResult> PostVideoAlbum(VideoAlbumDto videoAlbumDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var videoAlbum = DtoToEntityIMapper.Map<VideoAlbumDto, VideoAlbum>(videoAlbumDto); ////
            UoW.GetRepository<VideoAlbum>().Insert(videoAlbum);
            await UoW.SaveAsync();

            return CreatedAtRoute("DefaultApi", new { id = videoAlbum.ID }, videoAlbum);
        }

        // DELETE: api/VideoAlbums/5
        [ResponseType(typeof(VideoAlbum))]
        public async Task<IHttpActionResult> DeleteVideoAlbum(int id)
        {
            VideoAlbum videoAlbum = await UoW.GetRepository<VideoAlbum>().GetItemAsycn(e => e.ID == id);
            if (videoAlbum == null)
            {
                return NotFound();
            }

            UoW.GetRepository<VideoAlbum>().Delete(videoAlbum);
            await UoW.SaveAsync();

            return Ok(videoAlbum);
        }



        private bool VideoAlbumExists(int id)
        {
            return UoW.GetRepository<VideoAlbum>().GetItem(e => e.ID == id) != null;
        }
    }
}
