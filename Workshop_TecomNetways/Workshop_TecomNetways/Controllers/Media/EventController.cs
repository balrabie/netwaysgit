using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Workshop_TecomNetways.DTO;
using Workshop_TecomNetways.Models;
using Workshop_TecomNetways.Repository;

namespace Workshop_TecomNetways.Controllers
{

    public class EventController : ApiController
    {
        private UnitOfWork UoW = null;
        private IMapper EntityToDtoIMapper = null;
        private IMapper DtoToEntityIMapper = null;

        public EventController()
        {
            UoW = new UnitOfWork();
            InitializeMapping();

        }

        public EventController(UnitOfWork uoW)
        {
            UoW = uoW;
            InitializeMapping();
        }

        private void InitializeMapping()
        {
            EntityToDtoIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<Event, EventDto>())
                .CreateMapper();
            DtoToEntityIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<EventDto, Event>())
                .CreateMapper();
        }

        // GET: api/Events
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            var items = UoW.GetRepository<Event>().GetAll().ToList();
            var DTO = EntityToDtoIMapper.Map<List<Event>, List<EventDto>>(items).ToList();

            return Ok(DTO);
        }

        // GET: api/Events/5
        [ResponseType(typeof(Event))]
        public async Task<IHttpActionResult> GetEvent(int id)
        {
            Event item = await UoW.GetRepository<Event>().GetItemAsycn(e => e.ID == id);
            if (item == null)
            {
                return NotFound();
            }

            var DTO = EntityToDtoIMapper.Map<Event, EventDto>(item);
            return Ok(DTO);
        }

        // PUT: api/Events/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutEvent(int id, EventDto eventItemDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != eventItemDto.ID)
            {
                return BadRequest();
            }
            var eventItem = DtoToEntityIMapper.Map<EventDto, Event>(eventItemDto); ////
            UoW.GetRepository<Event>().ModifyEntityState(eventItem);

            try
            {
                await UoW.SaveAsync();
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

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Events
        [ResponseType(typeof(Event))]
        public async Task<IHttpActionResult> PostEvent(EventDto eventItemDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var eventItem = DtoToEntityIMapper.Map<EventDto, Event>(eventItemDto); ////
            UoW.GetRepository<Event>().Insert(eventItem);
            await UoW.SaveAsync();

            return CreatedAtRoute("DefaultApi", new { id = eventItem.ID }, eventItem);
        }

        // DELETE: api/Events/5
        [ResponseType(typeof(Event))]
        public async Task<IHttpActionResult> DeleteEvent(int id)
        {
            Event eventItem = await UoW.GetRepository<Event>().GetItemAsycn(e => e.ID == id);
            if (eventItem == null)
            {
                return NotFound();
            }

            UoW.GetRepository<Event>().Delete(eventItem);
            await UoW.SaveAsync();

            return Ok(eventItem);
        }



        private bool EventExists(int id)
        {
            return UoW.GetRepository<Event>().GetItem(e => e.ID == id) != null;
        }
    }
}
