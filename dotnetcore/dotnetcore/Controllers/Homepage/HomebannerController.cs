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
    public class HomebannerController : ControllerBase
    {
        private UnitOfWork uoW = null;
        private IRepository<Homebanner> repository = null;

        private IMapper EntityToDtoIMapper = null;
        private IMapper DtoToEntityIMapper = null;

        private void InitializeMapping()
        {
            EntityToDtoIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<Homebanner, HomebannerDto>())
                .CreateMapper();
            DtoToEntityIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<HomebannerDto, Homebanner>())
                .CreateMapper();
        }

        public HomebannerController()
        {
            uoW = new UnitOfWork();
            repository = uoW.GetRepository<Homebanner>();
            InitializeMapping();
        }

        // GET: api/Homebanner
        [HttpGet]
        public IEnumerable<HomebannerDto> Index()
        {
            List<HomebannerDto> homebannerDto = EntityToDtoIMapper
                .Map<List<Homebanner>, List<HomebannerDto>>(repository.GetAll().ToList())
                .ToList();

            return homebannerDto;
        }

        // GET: api/Homebanner/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HomebannerDto>> GetHomebanner([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var homebanner = await repository.GetAsync(a => a.ID == id);

            if (homebanner == null)
            {
                return NotFound();
            }

            HomebannerDto homebannerDto = EntityToDtoIMapper.Map<Homebanner, HomebannerDto>(homebanner);

            return Ok(homebannerDto);
        }

        // PUT: api/Homebanner/5
        [HttpPut("{id}")]
        public async Task<ActionResult<HomebannerDto>> PutHomebanner([FromRoute] int id, [FromBody] HomebannerDto homebannerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != homebannerDto.ID)
            {
                return BadRequest();
            }

            Homebanner homebanner = DtoToEntityIMapper.Map<HomebannerDto, Homebanner>(homebannerDto);

            repository.ModifyEntryState(homebanner, EntityState.Modified);

            try
            {
                await uoW.SaveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HomebannerExists(id))
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

        // POST: api/Homebanners
        [HttpPost]
        public async Task<ActionResult<HomebannerDto>> PostHomebanner([FromBody] HomebannerDto homebannerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Homebanner homebanner = DtoToEntityIMapper.Map<HomebannerDto, Homebanner>(homebannerDto);

            repository.Add(homebanner);
            await uoW.SaveAsync();

            return CreatedAtAction("GetHomebanner", new { id = homebanner.ID }, homebannerDto);
        }

        // DELETE: api/Homebanners/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<HomebannerDto>> DeleteHomebanner([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Homebanner homebanner = await repository.GetAsync(a => a.ID == id);

            if (homebanner == null)
            {
                return NotFound();
            }

            repository.Delete(homebanner);
            await uoW.SaveAsync();

            HomebannerDto homebannerDto = EntityToDtoIMapper.Map<Homebanner, HomebannerDto>(homebanner);

            return Ok(homebannerDto);
        }

        private bool HomebannerExists(int id)
        {
            return repository.Get(a => a.ID == id) != null;
        }
    }
}