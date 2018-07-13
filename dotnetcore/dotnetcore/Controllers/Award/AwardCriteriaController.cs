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
    public class AwardCriteriaController : ControllerBase
    {
        private UnitOfWork uoW = null;
        private IRepository<AwardCriteria> repository = null;

        private IMapper EntityToDtoIMapper = null;
        private IMapper DtoToEntityIMapper = null;

        private void InitializeMapping()
        {
            EntityToDtoIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<AwardCriteria, AwardCriteriaDto>())
                .CreateMapper();
            DtoToEntityIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<AwardCriteriaDto, AwardCriteria>())
                .CreateMapper();
        }

        public AwardCriteriaController()
        {
            uoW = new UnitOfWork();
            repository = uoW.GetRepository<AwardCriteria>();
            InitializeMapping();
        }

        // GET: api/AwardCriteria
        [HttpGet]
        public IEnumerable<AwardCriteriaDto> Index()
        {
            List<AwardCriteriaDto> awardCriteriaDto = EntityToDtoIMapper
                .Map<List<AwardCriteria>, List<AwardCriteriaDto>>(repository.GetAll().ToList())
                .ToList();

            return awardCriteriaDto;
        }

        // GET: api/AwardCriteria/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AwardCriteriaDto>> GetAwardCriteria([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var awardCriteria = await repository.GetAsync(a => a.ID == id);

            if (awardCriteria == null)
            {
                return NotFound();
            }

            AwardCriteriaDto awardCriteriaDto = EntityToDtoIMapper.Map<AwardCriteria, AwardCriteriaDto>(awardCriteria);

            return Ok(awardCriteriaDto);
        }

        // PUT: api/AwardCriteria/5
        [HttpPut("{id}")]
        public async Task<ActionResult<AwardCriteriaDto>> PutAwardCriteria([FromRoute] int id, [FromBody] AwardCriteriaDto awardCriteriaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != awardCriteriaDto.ID)
            {
                return BadRequest();
            }

            AwardCriteria awardCriteria = DtoToEntityIMapper.Map<AwardCriteriaDto, AwardCriteria>(awardCriteriaDto);

            repository.ModifyEntryState(awardCriteria, EntityState.Modified);

            try
            {
                await uoW.SaveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AwardCriteriaExists(id))
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

        // POST: api/AwardCriterias
        [HttpPost]
        public async Task<ActionResult<AwardCriteriaDto>> PostAwardCriteria([FromBody] AwardCriteriaDto awardCriteriaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            AwardCriteria awardCriteria = DtoToEntityIMapper.Map<AwardCriteriaDto, AwardCriteria>(awardCriteriaDto);

            repository.Add(awardCriteria);
            await uoW.SaveAsync();

            return CreatedAtAction("GetAwardCriteria", new { id = awardCriteria.ID }, awardCriteriaDto);
        }

        // DELETE: api/AwardCriterias/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<AwardCriteriaDto>> DeleteAwardCriteria([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            AwardCriteria awardCriteria = await repository.GetAsync(a => a.ID == id);

            if (awardCriteria == null)
            {
                return NotFound();
            }

            repository.Delete(awardCriteria);
            await uoW.SaveAsync();

            AwardCriteriaDto awardCriteriaDto = EntityToDtoIMapper.Map<AwardCriteria, AwardCriteriaDto>(awardCriteria);

            return Ok(awardCriteriaDto);
        }

        private bool AwardCriteriaExists(int id)
        {
            return repository.Get(a => a.ID == id) != null;
        }
    }
}