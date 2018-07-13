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
    public class FeedbackRequestController : ControllerBase
    {
        private UnitOfWork uoW = null;
        private IRepository<FeedbackRequest> repository = null;

        private IMapper EntityToDtoIMapper = null;
        private IMapper DtoToEntityIMapper = null;

        private void InitializeMapping()
        {
            EntityToDtoIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<FeedbackRequest, FeedbackRequestDto>())
                .CreateMapper();
            DtoToEntityIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<FeedbackRequestDto, FeedbackRequest>())
                .CreateMapper();
        }

        public FeedbackRequestController()
        {
            uoW = new UnitOfWork();
            repository = uoW.GetRepository<FeedbackRequest>();
            InitializeMapping();
        }

        // GET: api/FeedbackRequest
        [HttpGet]
        public IEnumerable<FeedbackRequestDto> Index()
        {
            List<FeedbackRequestDto> feedbackRequestDto = EntityToDtoIMapper
                .Map<List<FeedbackRequest>, List<FeedbackRequestDto>>(repository.GetAll().ToList())
                .ToList();

            return feedbackRequestDto;
        }

        // GET: api/FeedbackRequest/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FeedbackRequestDto>> GetFeedbackRequest([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var feedbackRequest = await repository.GetAsync(a => a.ID == id);

            if (feedbackRequest == null)
            {
                return NotFound();
            }

            FeedbackRequestDto feedbackRequestDto = EntityToDtoIMapper.Map<FeedbackRequest, FeedbackRequestDto>(feedbackRequest);

            return Ok(feedbackRequestDto);
        }

        // PUT: api/FeedbackRequest/5
        [HttpPut("{id}")]
        public async Task<ActionResult<FeedbackRequestDto>> PutFeedbackRequest([FromRoute] int id, [FromBody] FeedbackRequestDto feedbackRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != feedbackRequestDto.ID)
            {
                return BadRequest();
            }

            FeedbackRequest feedbackRequest = DtoToEntityIMapper.Map<FeedbackRequestDto, FeedbackRequest>(feedbackRequestDto);

            repository.ModifyEntryState(feedbackRequest, EntityState.Modified);

            try
            {
                await uoW.SaveAsync();
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

            return NoContent();
        }

        // POST: api/FeedbackRequests
        [HttpPost]
        public async Task<ActionResult<FeedbackRequestDto>> PostFeedbackRequest([FromBody] FeedbackRequestDto feedbackRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            FeedbackRequest feedbackRequest = DtoToEntityIMapper.Map<FeedbackRequestDto, FeedbackRequest>(feedbackRequestDto);

            repository.Add(feedbackRequest);
            await uoW.SaveAsync();

            return CreatedAtAction("GetFeedbackRequest", new { id = feedbackRequest.ID }, feedbackRequestDto);
        }

        // DELETE: api/FeedbackRequests/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<FeedbackRequestDto>> DeleteFeedbackRequest([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            FeedbackRequest feedbackRequest = await repository.GetAsync(a => a.ID == id);

            if (feedbackRequest == null)
            {
                return NotFound();
            }

            repository.Delete(feedbackRequest);
            await uoW.SaveAsync();

            FeedbackRequestDto feedbackRequestDto = EntityToDtoIMapper.Map<FeedbackRequest, FeedbackRequestDto>(feedbackRequest);

            return Ok(feedbackRequestDto);
        }

        private bool FeedbackRequestExists(int id)
        {
            return repository.Get(a => a.ID == id) != null;
        }
    }
}