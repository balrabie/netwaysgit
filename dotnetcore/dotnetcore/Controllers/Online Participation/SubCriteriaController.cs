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
    public class SubCriteriaController : ControllerBase
    {
        private UnitOfWork uoW = null;
        private IRepository<SubCriteria> repository = null;

        private IMapper EntityToDtoIMapper = null;
        private IMapper DtoToEntityIMapper = null;

        private void InitializeMapping()
        {
            EntityToDtoIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<SubCriteria, SubCriteriaDto>())
                .CreateMapper();
            DtoToEntityIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<SubCriteriaDto, SubCriteria>())
                .CreateMapper();
        }

        public SubCriteriaController()
        {
            uoW = new UnitOfWork();
            repository = uoW.GetRepository<SubCriteria>();
            InitializeMapping();
        }

        // GET: api/SubCriteria
        [HttpGet]
        public IEnumerable<SubCriteriaDto> Index()
        {
            List<SubCriteriaDto> subCriteriaDto = EntityToDtoIMapper
                .Map<List<SubCriteria>, List<SubCriteriaDto>>(repository.GetAll().ToList())
                .ToList();

            return subCriteriaDto;
        }

        // GET: api/SubCriteria/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SubCriteriaDto>> GetSubCriteria([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var subCriteria = await repository.GetAsync(a => a.ID == id);

            if (subCriteria == null)
            {
                return NotFound();
            }

            SubCriteriaDto subCriteriaDto = EntityToDtoIMapper.Map<SubCriteria, SubCriteriaDto>(subCriteria);

            return Ok(subCriteriaDto);
        }

        // PUT: api/SubCriteria/5
        [HttpPut("{id}")]
        public async Task<ActionResult<SubCriteriaDto>> PutSubCriteria([FromRoute] int id, [FromBody] SubCriteriaDto subCriteriaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != subCriteriaDto.ID)
            {
                return BadRequest();
            }

            SubCriteria subCriteria = DtoToEntityIMapper.Map<SubCriteriaDto, SubCriteria>(subCriteriaDto);

            repository.ModifyEntryState(subCriteria, EntityState.Modified);

            try
            {
                await uoW.SaveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubCriteriaExists(id))
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

        // POST: api/SubCriterias
        [HttpPost]
        public async Task<ActionResult<SubCriteriaDto>> PostSubCriteria([FromBody] SubCriteriaDto subCriteriaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            SubCriteria subCriteria = DtoToEntityIMapper.Map<SubCriteriaDto, SubCriteria>(subCriteriaDto);

            repository.Add(subCriteria);
            await uoW.SaveAsync();

            return CreatedAtAction("GetSubCriteria", new { id = subCriteria.ID }, subCriteriaDto);
        }

        // DELETE: api/SubCriterias/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<SubCriteriaDto>> DeleteSubCriteria([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            SubCriteria subCriteria = await repository.GetAsync(a => a.ID == id);

            if (subCriteria == null)
            {
                return NotFound();
            }

            repository.Delete(subCriteria);
            await uoW.SaveAsync();

            SubCriteriaDto subCriteriaDto = EntityToDtoIMapper.Map<SubCriteria, SubCriteriaDto>(subCriteria);

            return Ok(subCriteriaDto);
        }

        private bool SubCriteriaExists(int id)
        {
            return repository.Get(a => a.ID == id) != null;
        }
    }
}