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

    public class LocationController : ApiController
    {
        private UnitOfWork UoW = null;
        private IMapper EntityToDtoIMapper = null;
        private IMapper DtoToEntityIMapper = null;

        public LocationController()
        {
            UoW = new UnitOfWork();
            InitializeMapping();

        }

        public LocationController(UnitOfWork uoW)
        {
            UoW = uoW;
            InitializeMapping();
        }

        private void InitializeMapping()
        {
            EntityToDtoIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<Location, LocationDto>())
                .CreateMapper();
            DtoToEntityIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<LocationDto, Location>())
                .CreateMapper();
        }

        // GET: api/Locations
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            var items = UoW.GetRepository<Location>().GetAll().ToList();
            var DTO = EntityToDtoIMapper.Map<List<Location>, List<LocationDto>>(items).ToList();

            return Ok(DTO);
        }

        // GET: api/Locations/5
        [ResponseType(typeof(Location))]
        public async Task<IHttpActionResult> GetLocation(int id)
        {
            Location item = await UoW.GetRepository<Location>().GetItemAsycn(e => e.ID == id);
            if (item == null)
            {
                return NotFound();
            }

            var DTO = EntityToDtoIMapper.Map<Location, LocationDto>(item);
            return Ok(DTO);
        }

        // PUT: api/Locations/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutLocation(int id, LocationDto locationDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != locationDto.ID)
            {
                return BadRequest();
            }
            var location = DtoToEntityIMapper.Map<LocationDto, Location>(locationDto); ////
            UoW.GetRepository<Location>().ModifyEntityState(location);

            try
            {
                await UoW.SaveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LocationExists(id))
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

        // POST: api/Locations
        [ResponseType(typeof(Location))]
        public async Task<IHttpActionResult> PostLocation(LocationDto locationDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var location = DtoToEntityIMapper.Map<LocationDto, Location>(locationDto); ////
            UoW.GetRepository<Location>().Insert(location);
            await UoW.SaveAsync();

            return CreatedAtRoute("DefaultApi", new { id = location.ID }, location);
        }

        // DELETE: api/Locations/5
        [ResponseType(typeof(Location))]
        public async Task<IHttpActionResult> DeleteLocation(int id)
        {
            Location location = await UoW.GetRepository<Location>().GetItemAsycn(e => e.ID == id);
            if (location == null)
            {
                return NotFound();
            }

            UoW.GetRepository<Location>().Delete(location);
            await UoW.SaveAsync();

            return Ok(location);
        }



        private bool LocationExists(int id)
        {
            return UoW.GetRepository<Location>().GetItem(e => e.ID == id) != null;
        }
    }
}
