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
    public class AwardController : ControllerBase
    {
        private UnitOfWork uoW = null;
        private IRepository<Award> repository = null;

        private IMapper EntityToDtoIMapper = null;
        private IMapper DtoToEntityIMapper = null;

        private void InitializeMapping()
        {
            EntityToDtoIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<Award, AwardDto>())
                .CreateMapper();
            DtoToEntityIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<AwardDto, Award>())
                .CreateMapper();
        }

        public AwardController()
        {
            uoW = new UnitOfWork();
            repository = uoW.GetRepository<Award>();
            InitializeMapping();
        }

        // GET: api/Award
        [HttpGet]
        public IEnumerable<AwardDto> Index()
        {
            List<AwardDto> awardDto = EntityToDtoIMapper
                .Map<List<Award>, List<AwardDto>>(repository.GetAll().ToList())
                .ToList();

            return awardDto;
        }

        // GET: api/Award/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AwardDto>> GetAward([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var award = await repository.GetAsync(a => a.ID == id);

            if (award == null)
            {
                return NotFound();
            }

            AwardDto awardDto = EntityToDtoIMapper.Map<Award, AwardDto>(award);

            return Ok(awardDto);
        }

        // PUT: api/Award/5
        [HttpPut("{id}")]
        public async Task<ActionResult<AwardDto>> PutAward([FromRoute] int id, [FromBody] AwardDto awardDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != awardDto.ID)
            {
                return BadRequest();
            }

            Award award = DtoToEntityIMapper.Map<AwardDto, Award>(awardDto);

            repository.ModifyEntryState(award, EntityState.Modified);

            try
            {
                await uoW.SaveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AwardExists(id))
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

        // POST: api/Awards
        [HttpPost]
        public async Task<ActionResult<AwardDto>> PostAward([FromBody] AwardDto awardDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Award award = DtoToEntityIMapper.Map<AwardDto, Award>(awardDto);

            repository.Add(award);
            await uoW.SaveAsync();

            return CreatedAtAction("GetAward", new { id = award.ID }, awardDto);
        }

        // DELETE: api/Awards/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<AwardDto>> DeleteAward([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Award award = await repository.GetAsync(a => a.ID == id);

            if (award == null)
            {
                return NotFound();
            }

            repository.Delete(award);
            await uoW.SaveAsync();

            AwardDto awardDto = EntityToDtoIMapper.Map<Award, AwardDto>(award);

            return Ok(awardDto);
        }

        private bool AwardExists(int id)
        {
            return repository.Get(a => a.ID == id) != null;
        }
    }
}