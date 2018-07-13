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
    public class CriteriaController : ControllerBase
    {
        private UnitOfWork uoW = null;
        private IRepository<Criteria> repository = null;

        private IMapper EntityToDtoIMapper = null;
        private IMapper DtoToEntityIMapper = null;

        private void InitializeMapping()
        {
            EntityToDtoIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<Criteria, CriteriaDto>())
                .CreateMapper();
            DtoToEntityIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<CriteriaDto, Criteria>())
                .CreateMapper();
        }

        public CriteriaController()
        {
            uoW = new UnitOfWork();
            repository = uoW.GetRepository<Criteria>();
            InitializeMapping();
        }

        // GET: api/Criteria
        [HttpGet]
        public IEnumerable<CriteriaDto> Index()
        {
            List<CriteriaDto> criteriaDto = EntityToDtoIMapper
                .Map<List<Criteria>, List<CriteriaDto>>(repository.GetAll().ToList())
                .ToList();

            return criteriaDto;
        }

        // GET: api/Criteria/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CriteriaDto>> GetCriteria([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var criteria = await repository.GetAsync(a => a.ID == id);

            if (criteria == null)
            {
                return NotFound();
            }

            CriteriaDto criteriaDto = EntityToDtoIMapper.Map<Criteria, CriteriaDto>(criteria);

            return Ok(criteriaDto);
        }

        // PUT: api/Criteria/5
        [HttpPut("{id}")]
        public async Task<ActionResult<CriteriaDto>> PutCriteria([FromRoute] int id, [FromBody] CriteriaDto criteriaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != criteriaDto.ID)
            {
                return BadRequest();
            }

            Criteria criteria = DtoToEntityIMapper.Map<CriteriaDto, Criteria>(criteriaDto);

            repository.ModifyEntryState(criteria, EntityState.Modified);

            try
            {
                await uoW.SaveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CriteriaExists(id))
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

        // POST: api/Criterias
        [HttpPost]
        public async Task<ActionResult<CriteriaDto>> PostCriteria([FromBody] CriteriaDto criteriaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Criteria criteria = DtoToEntityIMapper.Map<CriteriaDto, Criteria>(criteriaDto);

            repository.Add(criteria);
            await uoW.SaveAsync();

            return CreatedAtAction("GetCriteria", new { id = criteria.ID }, criteriaDto);
        }

        // DELETE: api/Criterias/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CriteriaDto>> DeleteCriteria([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Criteria criteria = await repository.GetAsync(a => a.ID == id);

            if (criteria == null)
            {
                return NotFound();
            }

            repository.Delete(criteria);
            await uoW.SaveAsync();

            CriteriaDto criteriaDto = EntityToDtoIMapper.Map<Criteria, CriteriaDto>(criteria);

            return Ok(criteriaDto);
        }

        private bool CriteriaExists(int id)
        {
            return repository.Get(a => a.ID == id) != null;
        }
    }
}