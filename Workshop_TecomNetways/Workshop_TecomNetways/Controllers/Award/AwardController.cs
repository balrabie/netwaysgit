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

    public class AwardController : ApiController
    {
        private UnitOfWork UoW = null;
        private IMapper EntityToDtoIMapper = null;
        private IMapper DtoToEntityIMapper = null;

        public AwardController()
        {
            UoW = new UnitOfWork();
            InitializeMapping();

        }

        public AwardController(UnitOfWork uoW)
        {
            UoW = uoW;
            InitializeMapping();
        }

        private void InitializeMapping()
        {
            EntityToDtoIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<Award, AwardDto>())
                .CreateMapper();
            DtoToEntityIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<AwardDto, Award>())
                .CreateMapper();
        }

        // GET: api/Awards
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            var items = UoW.GetRepository<Award>().GetAll().ToList();
            var DTO = EntityToDtoIMapper.Map<List<Award>, List<AwardDto>>(items).ToList();

            return Ok(DTO);
        }

        // GET: api/Awards/5
        [ResponseType(typeof(Award))]
        public async Task<IHttpActionResult> GetAward(int id)
        {
            Award item = await UoW.GetRepository<Award>().GetItemAsycn(e => e.ID == id);
            if (item == null)
            {
                return NotFound();
            }

            var DTO = EntityToDtoIMapper.Map<Award, AwardDto>(item);
            return Ok(DTO);
        }

        // PUT: api/Awards/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutAward(int id, AwardDto awardDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != awardDto.ID)
            {
                return BadRequest();
            }
            var award = DtoToEntityIMapper.Map<AwardDto, Award>(awardDto); ////
            UoW.GetRepository<Award>().ModifyEntityState(award);

            try
            {
                await UoW.SaveAsync();
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

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Awards
        [ResponseType(typeof(Award))]
        public async Task<IHttpActionResult> PostAward(AwardDto awardDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var award = DtoToEntityIMapper.Map<AwardDto, Award>(awardDto); ////
            UoW.GetRepository<Award>().Insert(award);
            await UoW.SaveAsync();

            return CreatedAtRoute("DefaultApi", new { id = award.ID }, award);
        }

        // DELETE: api/Awards/5
        [ResponseType(typeof(Award))]
        public async Task<IHttpActionResult> DeleteAward(int id)
        {
            Award award = await UoW.GetRepository<Award>().GetItemAsycn(e => e.ID == id);
            if (award == null)
            {
                return NotFound();
            }

            UoW.GetRepository<Award>().Delete(award);
            await UoW.SaveAsync();

            return Ok(award);
        }



        private bool AwardExists(int id)
        {
            return UoW.GetRepository<Award>().GetItem(e => e.ID == id) != null;
        }
    }
}
