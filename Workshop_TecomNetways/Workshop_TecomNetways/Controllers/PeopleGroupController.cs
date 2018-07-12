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

    public class PeopleGroupController : ApiController
    {
        private UnitOfWork UoW = null;
        private IMapper EntityToDtoIMapper = null;
        private IMapper DtoToEntityIMapper = null;

        public PeopleGroupController()
        {
            UoW = new UnitOfWork();
            InitializeMapping();

        }

        public PeopleGroupController(UnitOfWork uoW)
        {
            UoW = uoW;
            InitializeMapping();
        }

        private void InitializeMapping()
        {
            EntityToDtoIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<PeopleGroup, PeopleGroupDto>())
                .CreateMapper();
            DtoToEntityIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<PeopleGroupDto, PeopleGroup>())
                .CreateMapper();
        }

        // GET: api/PeopleGroups
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            var items = UoW.GetRepository<PeopleGroup>().GetAll().ToList();
            var DTO = EntityToDtoIMapper.Map<List<PeopleGroup>, List<PeopleGroupDto>>(items).ToList();

            return Ok(DTO);
        }

        // GET: api/PeopleGroups/5
        [ResponseType(typeof(PeopleGroup))]
        public async Task<IHttpActionResult> GetPeopleGroup(int id)
        {
            PeopleGroup item = await UoW.GetRepository<PeopleGroup>().GetItemAsycn(e => e.ID == id);
            if (item == null)
            {
                return NotFound();
            }

            var DTO = EntityToDtoIMapper.Map<PeopleGroup, PeopleGroupDto>(item);
            return Ok(DTO);
        }

        // PUT: api/PeopleGroups/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPeopleGroup(int id, PeopleGroupDto peopleGroupDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != peopleGroupDto.ID)
            {
                return BadRequest();
            }
            var peopleGroup = DtoToEntityIMapper.Map<PeopleGroupDto, PeopleGroup>(peopleGroupDto); ////
            UoW.GetRepository<PeopleGroup>().ModifyEntityState(peopleGroup);

            try
            {
                await UoW.SaveAsync();
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

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/PeopleGroups
        [ResponseType(typeof(PeopleGroup))]
        public async Task<IHttpActionResult> PostPeopleGroup(PeopleGroupDto peopleGroupDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var peopleGroup = DtoToEntityIMapper.Map<PeopleGroupDto, PeopleGroup>(peopleGroupDto); ////
            UoW.GetRepository<PeopleGroup>().Insert(peopleGroup);
            await UoW.SaveAsync();

            return CreatedAtRoute("DefaultApi", new { id = peopleGroup.ID }, peopleGroup);
        }

        // DELETE: api/PeopleGroups/5
        [ResponseType(typeof(PeopleGroup))]
        public async Task<IHttpActionResult> DeletePeopleGroup(int id)
        {
            PeopleGroup peopleGroup = await UoW.GetRepository<PeopleGroup>().GetItemAsycn(e => e.ID == id);
            if (peopleGroup == null)
            {
                return NotFound();
            }

            UoW.GetRepository<PeopleGroup>().Delete(peopleGroup);
            await UoW.SaveAsync();

            return Ok(peopleGroup);
        }



        private bool PeopleGroupExists(int id)
        {
            return UoW.GetRepository<PeopleGroup>().GetItem(e => e.ID == id) != null;
        }
    }
}
