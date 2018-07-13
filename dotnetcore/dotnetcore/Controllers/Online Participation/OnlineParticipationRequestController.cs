using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using dotnetcore.Data;
using AutoMapper;
using Services;

namespace dotnetcore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OnlineParticipationRequestController : ControllerBase
    {
        private UnitOfWork uoW = null;
        private IRepository<OnlineParticipationRequest> repository = null;
        ServiceConfiguration emailConfig = null;
        private IMapper EntityToDtoIMapper = null;
        private IMapper DtoToEntityIMapper = null;

        private void InitializeMapping()
        {
            EntityToDtoIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<OnlineParticipationRequest, OnlineParticipationRequestDto>())
                .CreateMapper();
            DtoToEntityIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<OnlineParticipationRequestDto, OnlineParticipationRequest>())
                .CreateMapper();
        }

        public OnlineParticipationRequestController()
        {
            uoW = new UnitOfWork();
            repository = uoW.GetRepository<OnlineParticipationRequest>();
            InitializeMapping();
            emailConfig = new ServiceConfiguration();
        }

        // GET: api/OnlineParticipationRequest
        [HttpGet]
        public IEnumerable<OnlineParticipationRequestDto> Index()
        {
            List<OnlineParticipationRequestDto> onlineParticipationRequestDto = EntityToDtoIMapper
                .Map<List<OnlineParticipationRequest>, List<OnlineParticipationRequestDto>>(repository.GetAll().ToList())
                .ToList();

            return onlineParticipationRequestDto;
        }

        // GET: api/OnlineParticipationRequest/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OnlineParticipationRequestDto>> GetOnlineParticipationRequest([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var onlineParticipationRequest = await repository.GetAsync(a => a.ID == id);

            if (onlineParticipationRequest == null)
            {
                return NotFound();
            }

            OnlineParticipationRequestDto onlineParticipationRequestDto = EntityToDtoIMapper.Map<OnlineParticipationRequest, OnlineParticipationRequestDto>(onlineParticipationRequest);

            return Ok(onlineParticipationRequestDto);
        }

        // PUT: api/OnlineParticipationRequest/5
        [HttpPut("{id}")]
        public async Task<ActionResult<OnlineParticipationRequestDto>> PutOnlineParticipationRequest([FromRoute] int id, [FromBody] OnlineParticipationRequestDto onlineParticipationRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != onlineParticipationRequestDto.ID)
            {
                return BadRequest();
            }

            OnlineParticipationRequest onlineParticipationRequest = DtoToEntityIMapper.Map<OnlineParticipationRequestDto, OnlineParticipationRequest>(onlineParticipationRequestDto);

            repository.ModifyEntryState(onlineParticipationRequest, EntityState.Modified);

            try
            {
                await uoW.SaveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OnlineParticipationRequestExists(id))
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

        // POST: api/OnlineParticipationRequests
        [HttpPost]
        public async Task<ActionResult<OnlineParticipationRequestDto>> PostOnlineParticipationRequest([FromBody] OnlineParticipationRequestDto onlineParticipationRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            OnlineParticipationRequest onlineParticipationRequest = DtoToEntityIMapper.Map<OnlineParticipationRequestDto, OnlineParticipationRequest>(onlineParticipationRequestDto);

            repository.Add(onlineParticipationRequest);
            await uoW.SaveAsync();

            string toEmail = (await uoW.GetRepository<User>()
                .GetAsync(e => e.ID == onlineParticipationRequestDto.UserID))
                .Email;

            emailConfig.Details(from: "donotreplybrd@gmail.com",
                to: toEmail,
                fromPassword: "brdworkshop_96##"
                );

            emailConfig.EmailManager.SendMessage("Online Participation form",
                "You have successfully participated in the online form");

            return CreatedAtAction("GetOnlineParticipationRequest", new { id = onlineParticipationRequest.ID }, onlineParticipationRequestDto);
        }

        // DELETE: api/OnlineParticipationRequests/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<OnlineParticipationRequestDto>> DeleteOnlineParticipationRequest([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            OnlineParticipationRequest onlineParticipationRequest = await repository.GetAsync(a => a.ID == id);

            if (onlineParticipationRequest == null)
            {
                return NotFound();
            }

            repository.Delete(onlineParticipationRequest);
            await uoW.SaveAsync();

            OnlineParticipationRequestDto onlineParticipationRequestDto = EntityToDtoIMapper.Map<OnlineParticipationRequest, OnlineParticipationRequestDto>(onlineParticipationRequest);

            return Ok(onlineParticipationRequestDto);
        }

        private bool OnlineParticipationRequestExists(int id)
        {
            return repository.Get(a => a.ID == id) != null;
        }

        private string GenerateTrackingCode(string CountryCode)
        {
            return CountryCode + " : " + DateTime.UtcNow
                .ToString("yyyy/MM/dd : HH:mm:ss");
        }
    }
}