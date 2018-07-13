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
    public class EventController : ControllerBase
    {
        private UnitOfWork uoW = null;
        private IRepository<Event> repository = null;

        private IMapper EntityToDtoIMapper = null;
        private IMapper DtoToEntityIMapper = null;

        private void InitializeMapping()
        {
            EntityToDtoIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<Event, EventDto>())
                .CreateMapper();
            DtoToEntityIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<EventDto, Event>())
                .CreateMapper();
        }

        public EventController()
        {
            uoW = new UnitOfWork();
            repository = uoW.GetRepository<Event>();
            InitializeMapping();
        }

        // GET: api/Event
        [HttpGet]
        public IEnumerable<EventDto> Index()
        {
            List<EventDto> eventInstanceDto = EntityToDtoIMapper
                .Map<List<Event>, List<EventDto>>(repository.GetAll().ToList())
                .ToList();

            return eventInstanceDto;
        }

        // GET: api/Event/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EventDto>> GetEvent([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var eventInstance = await repository.GetAsync(a => a.ID == id);

            if (eventInstance == null)
            {
                return NotFound();
            }

            EventDto eventInstanceDto = EntityToDtoIMapper.Map<Event, EventDto>(eventInstance);

            return Ok(eventInstanceDto);
        }

        // PUT: api/Event/5
        [HttpPut("{id}")]
        public async Task<ActionResult<EventDto>> PutEvent([FromRoute] int id, [FromBody] EventDto eventInstanceDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != eventInstanceDto.ID)
            {
                return BadRequest();
            }

            Event eventInstance = DtoToEntityIMapper.Map<EventDto, Event>(eventInstanceDto);

            repository.ModifyEntryState(eventInstance, EntityState.Modified);

            try
            {
                await uoW.SaveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(id))
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

        // POST: api/Events
        [HttpPost]
        public async Task<ActionResult<EventDto>> PostEvent([FromBody] EventDto eventInstanceDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Event eventInstance = DtoToEntityIMapper.Map<EventDto, Event>(eventInstanceDto);

            repository.Add(eventInstance);
            await uoW.SaveAsync();

            return CreatedAtAction("GetEvent", new { id = eventInstance.ID }, eventInstanceDto);
        }

        // DELETE: api/Events/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<EventDto>> DeleteEvent([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Event eventInstance = await repository.GetAsync(a => a.ID == id);

            if (eventInstance == null)
            {
                return NotFound();
            }

            repository.Delete(eventInstance);
            await uoW.SaveAsync();

            EventDto eventInstanceDto = EntityToDtoIMapper.Map<Event, EventDto>(eventInstance);

            return Ok(eventInstanceDto);
        }

        private bool EventExists(int id)
        {
            return repository.Get(a => a.ID == id) != null;
        }
    }
}