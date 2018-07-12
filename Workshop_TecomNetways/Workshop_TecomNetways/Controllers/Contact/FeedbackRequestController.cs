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

    public class FeedbackRequestController : ApiController
    {
        private UnitOfWork UoW = null;
        private IMapper EntityToDtoIMapper = null;
        private IMapper DtoToEntityIMapper = null;

        public FeedbackRequestController()
        {
            UoW = new UnitOfWork();
            InitializeMapping();

        }

        public FeedbackRequestController(UnitOfWork uoW)
        {
            UoW = uoW;
            InitializeMapping();
        }

        private void InitializeMapping()
        {
            EntityToDtoIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<FeedbackRequest, FeedbackRequestDto>())
                .CreateMapper();
            DtoToEntityIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<FeedbackRequestDto, FeedbackRequest>())
                .CreateMapper();
        }

        // GET: api/FeedbackRequests
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            var items = UoW.GetRepository<FeedbackRequest>().GetAll().ToList();
            var DTO = EntityToDtoIMapper.Map<List<FeedbackRequest>, List<FeedbackRequestDto>>(items).ToList();

            return Ok(DTO);
        }

        // GET: api/FeedbackRequests/5
        [ResponseType(typeof(FeedbackRequest))]
        public async Task<IHttpActionResult> GetFeedbackRequest(int id)
        {
            FeedbackRequest item = await UoW.GetRepository<FeedbackRequest>().GetItemAsycn(e => e.ID == id);
            if (item == null)
            {
                return NotFound();
            }

            var DTO = EntityToDtoIMapper.Map<FeedbackRequest, FeedbackRequestDto>(item);
            return Ok(DTO);
        }

        // PUT: api/FeedbackRequests/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutFeedbackRequest(int id, FeedbackRequestDto feedbackRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != feedbackRequestDto.ID)
            {
                return BadRequest();
            }
            var feedbackRequest = DtoToEntityIMapper.Map<FeedbackRequestDto, FeedbackRequest>(feedbackRequestDto); ////
            UoW.GetRepository<FeedbackRequest>().ModifyEntityState(feedbackRequest);

            try
            {
                await UoW.SaveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FeedbackRequestExists(id))
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

        // POST: api/FeedbackRequests
        [ResponseType(typeof(FeedbackRequest))]
        public async Task<IHttpActionResult> PostFeedbackRequest(FeedbackRequestDto feedbackRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var feedbackRequest = DtoToEntityIMapper.Map<FeedbackRequestDto, FeedbackRequest>(feedbackRequestDto); ////
            UoW.GetRepository<FeedbackRequest>().Insert(feedbackRequest);
            await UoW.SaveAsync();

            return CreatedAtRoute("DefaultApi", new { id = feedbackRequest.ID }, feedbackRequest);
        }

        // DELETE: api/FeedbackRequests/5
        [ResponseType(typeof(FeedbackRequest))]
        public async Task<IHttpActionResult> DeleteFeedbackRequest(int id)
        {
            FeedbackRequest feedbackRequest = await UoW.GetRepository<FeedbackRequest>().GetItemAsycn(e => e.ID == id);
            if (feedbackRequest == null)
            {
                return NotFound();
            }

            UoW.GetRepository<FeedbackRequest>().Delete(feedbackRequest);
            await UoW.SaveAsync();

            return Ok(feedbackRequest);
        }



        private bool FeedbackRequestExists(int id)
        {
            return UoW.GetRepository<FeedbackRequest>().GetItem(e => e.ID == id) != null;
        }
    }
}
