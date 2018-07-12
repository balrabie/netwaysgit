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

    public class AwardQuoteController : ApiController
    {
        private UnitOfWork UoW = null;
        private IMapper EntityToDtoIMapper = null;
        private IMapper DtoToEntityIMapper = null;

        public AwardQuoteController()
        {
            UoW = new UnitOfWork();
            InitializeMapping();

        }

        public AwardQuoteController(UnitOfWork uoW)
        {
            UoW = uoW;
            InitializeMapping();
        }

        private void InitializeMapping()
        {
            EntityToDtoIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<AwardQuote, AwardQuoteDto>())
                .CreateMapper();
            DtoToEntityIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<AwardQuoteDto, AwardQuote>())
                .CreateMapper();
        }

        // GET: api/AwardQuotes
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            var items = UoW.GetRepository<AwardQuote>().GetAll().ToList();
            var DTO = EntityToDtoIMapper.Map<List<AwardQuote>, List<AwardQuoteDto>>(items).ToList();

            return Ok(DTO);
        }

        // GET: api/AwardQuotes/5
        [ResponseType(typeof(AwardQuote))]
        public async Task<IHttpActionResult> GetAwardQuote(int id)
        {
            AwardQuote item = await UoW.GetRepository<AwardQuote>().GetItemAsycn(e => e.ID == id);
            if (item == null)
            {
                return NotFound();
            }

            var DTO = EntityToDtoIMapper.Map<AwardQuote, AwardQuoteDto>(item);
            return Ok(DTO);
        }

        // PUT: api/AwardQuotes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutAwardQuote(int id, AwardQuoteDto awardQuoteDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != awardQuoteDto.ID)
            {
                return BadRequest();
            }
            var awardQuote = DtoToEntityIMapper.Map<AwardQuoteDto, AwardQuote>(awardQuoteDto); ////
            UoW.GetRepository<AwardQuote>().ModifyEntityState(awardQuote);

            try
            {
                await UoW.SaveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AwardQuoteExists(id))
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

        // POST: api/AwardQuotes
        [ResponseType(typeof(AwardQuote))]
        public async Task<IHttpActionResult> PostAwardQuote(AwardQuoteDto awardQuoteDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var awardQuote = DtoToEntityIMapper.Map<AwardQuoteDto, AwardQuote>(awardQuoteDto); ////
            UoW.GetRepository<AwardQuote>().Insert(awardQuote);
            await UoW.SaveAsync();

            return CreatedAtRoute("DefaultApi", new { id = awardQuote.ID }, awardQuote);
        }

        // DELETE: api/AwardQuotes/5
        [ResponseType(typeof(AwardQuote))]
        public async Task<IHttpActionResult> DeleteAwardQuote(int id)
        {
            AwardQuote awardQuote = await UoW.GetRepository<AwardQuote>().GetItemAsycn(e => e.ID == id);
            if (awardQuote == null)
            {
                return NotFound();
            }

            UoW.GetRepository<AwardQuote>().Delete(awardQuote);
            await UoW.SaveAsync();

            return Ok(awardQuote);
        }



        private bool AwardQuoteExists(int id)
        {
            return UoW.GetRepository<AwardQuote>().GetItem(e => e.ID == id) != null;
        }
    }
}
