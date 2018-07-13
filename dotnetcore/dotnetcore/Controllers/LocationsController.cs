using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using dotnetcore.Data;
using AutoMapper;

namespace dotnetcore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private UnitOfWork uoW = null;
        private IRepository<Location> repository = null;

        private IMapper EntityToDtoIMapper = null;
        private IMapper DtoToEntityIMapper = null;

        private void InitializeMapping()
        {
            EntityToDtoIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<Location, LocationDto>())
                .CreateMapper();
            DtoToEntityIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<LocationDto, Location>())
                .CreateMapper();
        }

        public LocationController()
        {
            uoW = new UnitOfWork();
            repository = uoW.GetRepository<Location>();
            InitializeMapping();
        }

        // GET: api/Location
        [HttpGet]
        public IEnumerable<LocationDto> Index()
        {
            List<LocationDto> locationDto = EntityToDtoIMapper
                .Map<List<Location>, List<LocationDto>>(repository.GetAll().ToList())
                .ToList();

            return locationDto;
        }

        // GET: api/Location/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LocationDto>> GetLocation([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var location = await repository.GetAsync(a => a.ID == id);

            if (location == null)
            {
                return NotFound();
            }

            LocationDto locationDto = EntityToDtoIMapper.Map<Location, LocationDto>(location);

            return Ok(locationDto);
        }

        // PUT: api/Location/5
        [HttpPut("{id}")]
        public async Task<ActionResult<LocationDto>> PutLocation([FromRoute] int id, [FromBody] LocationDto locationDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != locationDto.ID)
            {
                return BadRequest();
            }

            Location location = DtoToEntityIMapper.Map<LocationDto, Location>(locationDto);

            repository.ModifyEntryState(location, EntityState.Modified);

            try
            {
                await uoW.SaveAsync();
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

            return NoContent();
        }

        // POST: api/Locations
        [HttpPost]
        public async Task<ActionResult<LocationDto>> PostLocation([FromBody] LocationDto locationDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Location location = DtoToEntityIMapper.Map<LocationDto, Location>(locationDto);

            repository.Add(location);
            await uoW.SaveAsync();

            return CreatedAtAction("GetLocation", new { id = location.ID }, locationDto);
        }

        // DELETE: api/Locations/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<LocationDto>> DeleteLocation([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Location location = await repository.GetAsync(a => a.ID == id);

            if (location == null)
            {
                return NotFound();
            }

            repository.Delete(location);
            await uoW.SaveAsync();

            LocationDto locationDto = EntityToDtoIMapper.Map<Location, LocationDto>(location);

            return Ok(locationDto);
        }

        private bool LocationExists(int id)
        {
            return repository.Get(a => a.ID == id) != null;
        }
    }
}