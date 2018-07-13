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
    public class NationalityController : ControllerBase
    {
        private UnitOfWork uoW = null;
        private IRepository<Nationality> repository = null;

        private IMapper EntityToDtoIMapper = null;
        private IMapper DtoToEntityIMapper = null;

        private void InitializeMapping()
        {
            EntityToDtoIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<Nationality, NationalityDto>())
                .CreateMapper();
            DtoToEntityIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<NationalityDto, Nationality>())
                .CreateMapper();
        }

        public NationalityController()
        {
            uoW = new UnitOfWork();
            repository = uoW.GetRepository<Nationality>();
            InitializeMapping();
        }

        // GET: api/Nationality
        [HttpGet]
        public IEnumerable<NationalityDto> Index()
        {
            List<NationalityDto> nationalityDto = EntityToDtoIMapper
                .Map<List<Nationality>, List<NationalityDto>>(repository.GetAll().ToList())
                .ToList();

            return nationalityDto;
        }

        // GET: api/Nationality/5
        [HttpGet("{id}")]
        public async Task<ActionResult<NationalityDto>> GetNationality([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var nationality = await repository.GetAsync(a => a.ID == id);

            if (nationality == null)
            {
                return NotFound();
            }

            NationalityDto nationalityDto = EntityToDtoIMapper.Map<Nationality, NationalityDto>(nationality);

            return Ok(nationalityDto);
        }

        // PUT: api/Nationality/5
        [HttpPut("{id}")]
        public async Task<ActionResult<NationalityDto>> PutNationality([FromRoute] int id, [FromBody] NationalityDto nationalityDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != nationalityDto.ID)
            {
                return BadRequest();
            }

            Nationality nationality = DtoToEntityIMapper.Map<NationalityDto, Nationality>(nationalityDto);

            repository.ModifyEntryState(nationality, EntityState.Modified);

            try
            {
                await uoW.SaveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NationalityExists(id))
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

        // POST: api/Nationalitys
        [HttpPost]
        public async Task<ActionResult<NationalityDto>> PostNationality([FromBody] NationalityDto nationalityDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Nationality nationality = DtoToEntityIMapper.Map<NationalityDto, Nationality>(nationalityDto);

            repository.Add(nationality);
            await uoW.SaveAsync();

            return CreatedAtAction("GetNationality", new { id = nationality.ID }, nationalityDto);
        }

        // DELETE: api/Nationalitys/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<NationalityDto>> DeleteNationality([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Nationality nationality = await repository.GetAsync(a => a.ID == id);

            if (nationality == null)
            {
                return NotFound();
            }

            repository.Delete(nationality);
            await uoW.SaveAsync();

            NationalityDto nationalityDto = EntityToDtoIMapper.Map<Nationality, NationalityDto>(nationality);

            return Ok(nationalityDto);
        }

        private bool NationalityExists(int id)
        {
            return repository.Get(a => a.ID == id) != null;
        }
    }
}