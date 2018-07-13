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
    public class CountryController : ControllerBase
    {
        private UnitOfWork uoW = null;
        private IRepository<Country> repository = null;

        private IMapper EntityToDtoIMapper = null;
        private IMapper DtoToEntityIMapper = null;

        private void InitializeMapping()
        {
            EntityToDtoIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<Country, CountryDto>())
                .CreateMapper();
            DtoToEntityIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<CountryDto, Country>())
                .CreateMapper();
        }

        public CountryController()
        {
            uoW = new UnitOfWork();
            repository = uoW.GetRepository<Country>();
            InitializeMapping();
        }

        // GET: api/Country
        [HttpGet]
        public IEnumerable<CountryDto> Index()
        {
            List<CountryDto> countryDto = EntityToDtoIMapper
                .Map<List<Country>, List<CountryDto>>(repository.GetAll().ToList())
                .ToList();

            return countryDto;
        }

        // GET: api/Country/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CountryDto>> GetCountry([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var country = await repository.GetAsync(a => a.ID == id);

            if (country == null)
            {
                return NotFound();
            }

            CountryDto countryDto = EntityToDtoIMapper.Map<Country, CountryDto>(country);

            return Ok(countryDto);
        }

        // PUT: api/Country/5
        [HttpPut("{id}")]
        public async Task<ActionResult<CountryDto>> PutCountry([FromRoute] int id, [FromBody] CountryDto countryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != countryDto.ID)
            {
                return BadRequest();
            }

            Country country = DtoToEntityIMapper.Map<CountryDto, Country>(countryDto);

            repository.ModifyEntryState(country, EntityState.Modified);

            try
            {
                await uoW.SaveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CountryExists(id))
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

        // POST: api/Countrys
        [HttpPost]
        public async Task<ActionResult<CountryDto>> PostCountry([FromBody] CountryDto countryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Country country = DtoToEntityIMapper.Map<CountryDto, Country>(countryDto);

            repository.Add(country);
            await uoW.SaveAsync();

            return CreatedAtAction("GetCountry", new { id = country.ID }, countryDto);
        }

        // DELETE: api/Countrys/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CountryDto>> DeleteCountry([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Country country = await repository.GetAsync(a => a.ID == id);

            if (country == null)
            {
                return NotFound();
            }

            repository.Delete(country);
            await uoW.SaveAsync();

            CountryDto countryDto = EntityToDtoIMapper.Map<Country, CountryDto>(country);

            return Ok(countryDto);
        }

        private bool CountryExists(int id)
        {
            return repository.Get(a => a.ID == id) != null;
        }
    }
}