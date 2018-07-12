using AutoMapper;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Workshop_TecomNetways.DTO;
using Workshop_TecomNetways.Models;
using Workshop_TecomNetways.Repository;

namespace Workshop_TecomNetways.Controllers
{

    public class AwardCriteriaController : ApiController
    {
        private UnitOfWork UoW = null;
        private IMapper EntityToDtoIMapper = null;
        private IMapper DtoToEntityIMapper = null;

        public AwardCriteriaController()
        {
            UoW = new UnitOfWork();
            InitializeMapping();

        }

        public AwardCriteriaController(UnitOfWork uoW)
        {
            UoW = uoW;
            InitializeMapping();
        }

        private void InitializeMapping()
        {
            EntityToDtoIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<AwardCriteria, AwardCriteriaDto>())
                .CreateMapper();
            DtoToEntityIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<AwardCriteriaDto, AwardCriteria>())
                .CreateMapper();
        }

        // GET: api/AwardCriterias
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            var items = UoW.GetRepository<AwardCriteria>().GetAll().ToList();
            var DTO = EntityToDtoIMapper.Map<List<AwardCriteria>, List<AwardCriteriaDto>>(items).ToList();

            return Ok(DTO);
        }

        // GET: api/AwardCriterias/5
        [ResponseType(typeof(AwardCriteria))]
        public async Task<IHttpActionResult> GetAwardCriteria(int id)
        {
            AwardCriteria item = await UoW.GetRepository<AwardCriteria>().GetItemAsycn(e => e.ID == id);
            if (item == null)
            {
                return NotFound();
            }

            var DTO = EntityToDtoIMapper.Map<AwardCriteria, AwardCriteriaDto>(item);
            return Ok(DTO);
        }

        // PUT: api/AwardCriterias/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutAwardCriteria(int id, AwardCriteriaDto awardCriteriaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != awardCriteriaDto.ID)
            {
                return BadRequest();
            }
            var awardCriteria = DtoToEntityIMapper.Map<AwardCriteriaDto, AwardCriteria>(awardCriteriaDto); ////
            UoW.GetRepository<AwardCriteria>().ModifyEntityState(awardCriteria);

            try
            {
                await UoW.SaveAsync();
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

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/AwardCriterias
        [ResponseType(typeof(AwardCriteria))]
        public async Task<IHttpActionResult> PostAwardCriteria(AwardCriteriaDto awardCriteriaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var awardCriteria = DtoToEntityIMapper.Map<AwardCriteriaDto, AwardCriteria>(awardCriteriaDto); ////
            UoW.GetRepository<AwardCriteria>().Insert(awardCriteria);
            await UoW.SaveAsync();

            return CreatedAtRoute("DefaultApi", new { id = awardCriteria.ID }, awardCriteria);
        }

        // DELETE: api/AwardCriterias/5
        [ResponseType(typeof(AwardCriteria))]
        public async Task<IHttpActionResult> DeleteAwardCriteria(int id)
        {
            AwardCriteria awardCriteria = await UoW.GetRepository<AwardCriteria>().GetItemAsycn(e => e.ID == id);
            if (awardCriteria == null)
            {
                return NotFound();
            }

            UoW.GetRepository<AwardCriteria>().Delete(awardCriteria);
            await UoW.SaveAsync();

            return Ok(awardCriteria);
        }



        private bool AwardCriteriaExists(int id)
        {
            return UoW.GetRepository<AwardCriteria>().GetItem(e => e.ID == id) != null;
        }
    }
}
