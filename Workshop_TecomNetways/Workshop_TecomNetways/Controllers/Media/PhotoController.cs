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

    public class PhotoController : ApiController
    {
        private UnitOfWork UoW = null;
        private IMapper EntityToDtoIMapper = null;
        private IMapper DtoToEntityIMapper = null;

        public PhotoController()
        {
            UoW = new UnitOfWork();
            InitializeMapping();

        }

        public PhotoController(UnitOfWork uoW)
        {
            UoW = uoW;
            InitializeMapping();
        }

        private void InitializeMapping()
        {
            EntityToDtoIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<Photo, PhotoDto>())
                .CreateMapper();
            DtoToEntityIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<PhotoDto, Photo>())
                .CreateMapper();
        }

        // GET: api/Photos
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            var items = UoW.GetRepository<Photo>().GetAll().ToList();
            var DTO = EntityToDtoIMapper.Map<List<Photo>, List<PhotoDto>>(items).ToList();

            return Ok(DTO);
        }

        // GET: api/Photos/5
        [ResponseType(typeof(Photo))]
        public async Task<IHttpActionResult> GetPhoto(int id)
        {
            Photo item = await UoW.GetRepository<Photo>().GetItemAsycn(e => e.ID == id);
            if (item == null)
            {
                return NotFound();
            }

            var DTO = EntityToDtoIMapper.Map<Photo, PhotoDto>(item);
            return Ok(DTO);
        }

        // PUT: api/Photos/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPhoto(int id, PhotoDto photoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != photoDto.ID)
            {
                return BadRequest();
            }
            var photo = DtoToEntityIMapper.Map<PhotoDto, Photo>(photoDto); ////
            UoW.GetRepository<Photo>().ModifyEntityState(photo);

            try
            {
                await UoW.SaveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PhotoExists(id))
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

        // POST: api/Photos
        [ResponseType(typeof(Photo))]
        public async Task<IHttpActionResult> PostPhoto(PhotoDto photoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var photo = DtoToEntityIMapper.Map<PhotoDto, Photo>(photoDto); ////
            UoW.GetRepository<Photo>().Insert(photo);
            await UoW.SaveAsync();

            return CreatedAtRoute("DefaultApi", new { id = photo.ID }, photo);
        }

        // DELETE: api/Photos/5
        [ResponseType(typeof(Photo))]
        public async Task<IHttpActionResult> DeletePhoto(int id)
        {
            Photo photo = await UoW.GetRepository<Photo>().GetItemAsycn(e => e.ID == id);
            if (photo == null)
            {
                return NotFound();
            }

            UoW.GetRepository<Photo>().Delete(photo);
            await UoW.SaveAsync();

            return Ok(photo);
        }



        private bool PhotoExists(int id)
        {
            return UoW.GetRepository<Photo>().GetItem(e => e.ID == id) != null;
        }
    }
}
