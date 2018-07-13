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
    public class PeopleGroupController : ControllerBase
    {
        private UnitOfWork uoW = null;
        private IRepository<PeopleGroup> repository = null;

        private IMapper EntityToDtoIMapper = null;
        private IMapper DtoToEntityIMapper = null;

        private void InitializeMapping()
        {
            EntityToDtoIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<PeopleGroup, PeopleGroupDto>())
                .CreateMapper();
            DtoToEntityIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<PeopleGroupDto, PeopleGroup>())
                .CreateMapper();
        }

        public PeopleGroupController()
        {
            uoW = new UnitOfWork();
            repository = uoW.GetRepository<PeopleGroup>();
            InitializeMapping();
        }

        // GET: api/PeopleGroup
        [HttpGet]
        public IEnumerable<PeopleGroupDto> Index()
        {
            List<PeopleGroupDto> peopleGroupDto = EntityToDtoIMapper
                .Map<List<PeopleGroup>, List<PeopleGroupDto>>(repository.GetAll().ToList())
                .ToList();

            return peopleGroupDto;
        }

        // GET: api/PeopleGroup/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PeopleGroupDto>> GetPeopleGroup([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var peopleGroup = await repository.GetAsync(a => a.ID == id);

            if (peopleGroup == null)
            {
                return NotFound();
            }

            PeopleGroupDto peopleGroupDto = EntityToDtoIMapper.Map<PeopleGroup, PeopleGroupDto>(peopleGroup);

            return Ok(peopleGroupDto);
        }

        // PUT: api/PeopleGroup/5
        [HttpPut("{id}")]
        public async Task<ActionResult<PeopleGroupDto>> PutPeopleGroup([FromRoute] int id, [FromBody] PeopleGroupDto peopleGroupDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != peopleGroupDto.ID)
            {
                return BadRequest();
            }

            PeopleGroup peopleGroup = DtoToEntityIMapper.Map<PeopleGroupDto, PeopleGroup>(peopleGroupDto);

            repository.ModifyEntryState(peopleGroup, EntityState.Modified);

            try
            {
                await uoW.SaveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PeopleGroupExists(id))
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

        // POST: api/PeopleGroups
        [HttpPost]
        public async Task<ActionResult<PeopleGroupDto>> PostPeopleGroup([FromBody] PeopleGroupDto peopleGroupDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            PeopleGroup peopleGroup = DtoToEntityIMapper.Map<PeopleGroupDto, PeopleGroup>(peopleGroupDto);

            repository.Add(peopleGroup);
            await uoW.SaveAsync();

            return CreatedAtAction("GetPeopleGroup", new { id = peopleGroup.ID }, peopleGroupDto);
        }

        // DELETE: api/PeopleGroups/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<PeopleGroupDto>> DeletePeopleGroup([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            PeopleGroup peopleGroup = await repository.GetAsync(a => a.ID == id);

            if (peopleGroup == null)
            {
                return NotFound();
            }

            repository.Delete(peopleGroup);
            await uoW.SaveAsync();

            PeopleGroupDto peopleGroupDto = EntityToDtoIMapper.Map<PeopleGroup, PeopleGroupDto>(peopleGroup);

            return Ok(peopleGroupDto);
        }

        private bool PeopleGroupExists(int id)
        {
            return repository.Get(a => a.ID == id) != null;
        }
    }
}