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

    public class PhotoAlbumController : ApiController
    {
        private UnitOfWork UoW = null;
        private IMapper EntityToDtoIMapper = null;
        private IMapper DtoToEntityIMapper = null;

        public PhotoAlbumController()
        {
            UoW = new UnitOfWork();
            InitializeMapping();

        }

        public PhotoAlbumController(UnitOfWork uoW)
        {
            UoW = uoW;
            InitializeMapping();
        }

        private void InitializeMapping()
        {
            EntityToDtoIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<PhotoAlbum, PhotoAlbumDto>())
                .CreateMapper();
            DtoToEntityIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<PhotoAlbumDto, PhotoAlbum>())
                .CreateMapper();
        }

        // GET: api/PhotoAlbums
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            var items = UoW.GetRepository<PhotoAlbum>().GetAll().ToList();
            var DTO = EntityToDtoIMapper.Map<List<PhotoAlbum>, List<PhotoAlbumDto>>(items).ToList();

            return Ok(DTO);
        }

        // GET: api/PhotoAlbums/5
        [ResponseType(typeof(PhotoAlbum))]
        public async Task<IHttpActionResult> GetPhotoAlbum(int id)
        {
            PhotoAlbum item = await UoW.GetRepository<PhotoAlbum>().GetItemAsycn(e => e.ID == id);
            if (item == null)
            {
                return NotFound();
            }

            var DTO = EntityToDtoIMapper.Map<PhotoAlbum, PhotoAlbumDto>(item);
            return Ok(DTO);
        }

        // PUT: api/PhotoAlbums/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPhotoAlbum(int id, PhotoAlbumDto photoAlbumDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != photoAlbumDto.ID)
            {
                return BadRequest();
            }
            var photoAlbum = DtoToEntityIMapper.Map<PhotoAlbumDto, PhotoAlbum>(photoAlbumDto); ////
            UoW.GetRepository<PhotoAlbum>().ModifyEntityState(photoAlbum);

            try
            {
                await UoW.SaveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PhotoAlbumExists(id))
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

        // POST: api/PhotoAlbums
        [ResponseType(typeof(PhotoAlbum))]
        public async Task<IHttpActionResult> PostPhotoAlbum(PhotoAlbumDto photoAlbumDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var photoAlbum = DtoToEntityIMapper.Map<PhotoAlbumDto, PhotoAlbum>(photoAlbumDto); ////
            UoW.GetRepository<PhotoAlbum>().Insert(photoAlbum);
            await UoW.SaveAsync();

            return CreatedAtRoute("DefaultApi", new { id = photoAlbum.ID }, photoAlbum);
        }

        // DELETE: api/PhotoAlbums/5
        [ResponseType(typeof(PhotoAlbum))]
        public async Task<IHttpActionResult> DeletePhotoAlbum(int id)
        {
            PhotoAlbum photoAlbum = await UoW.GetRepository<PhotoAlbum>().GetItemAsycn(e => e.ID == id);
            if (photoAlbum == null)
            {
                return NotFound();
            }

            UoW.GetRepository<PhotoAlbum>().Delete(photoAlbum);
            await UoW.SaveAsync();

            return Ok(photoAlbum);
        }



        private bool PhotoAlbumExists(int id)
        {
            return UoW.GetRepository<PhotoAlbum>().GetItem(e => e.ID == id) != null;
        }
    }
}
