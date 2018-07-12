using AutoMapper;
using Services;
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

    public class OnlineParticipationRequestController : ApiController
    {
        private UnitOfWork UoW = null;
        private IMapper EntityToDtoIMapper = null;
        private IMapper DtoToEntityIMapper = null;
        ServiceConfiguration emailConfig = new ServiceConfiguration();

        public OnlineParticipationRequestController()
        {
            UoW = new UnitOfWork();
            InitializeMapping();

        }

        public OnlineParticipationRequestController(UnitOfWork uoW)
        {
            UoW = uoW;
            InitializeMapping();
        }

        private void InitializeMapping()
        {
            EntityToDtoIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<OnlineParticipationRequest, OnlineParticipationRequestDto>())
                .CreateMapper();
            DtoToEntityIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<OnlineParticipationRequestDto, OnlineParticipationRequest>())
                .CreateMapper();
        }

        // GET: api/OnlineParticipationRequests
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            var items = UoW.GetRepository<OnlineParticipationRequest>().GetAll().ToList();
            var DTO = EntityToDtoIMapper.Map<List<OnlineParticipationRequest>, List<OnlineParticipationRequestDto>>(items).ToList();

            return Ok(DTO);
        }

        // GET: api/OnlineParticipationRequests/5
        [ResponseType(typeof(OnlineParticipationRequest))]
        public async Task<IHttpActionResult> GetOnlineParticipationRequest(int id)
        {
            OnlineParticipationRequest item = await UoW.GetRepository<OnlineParticipationRequest>().GetItemAsycn(e => e.ID == id);
            if (item == null)
            {
                return NotFound();
            }

            var DTO = EntityToDtoIMapper.Map<OnlineParticipationRequest, OnlineParticipationRequestDto>(item);
            return Ok(DTO);
        }


        // POST: api/OnlineParticipationRequests
        [ResponseType(typeof(OnlineParticipationRequest))]
        public async Task<IHttpActionResult> PostOnlineParticipationRequest(OnlineParticipationRequestDto onlineParticipationRequestDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var onlineParticipationRequest = DtoToEntityIMapper
                .Map<OnlineParticipationRequestDto, OnlineParticipationRequest>(onlineParticipationRequestDto); ////

            string countryCode = (await UoW.GetRepository<Country>()
                .GetItemAsycn(e => e.ID == onlineParticipationRequestDto.CountryID))
                .CountryCode;

            onlineParticipationRequest.TrackingCode = GenerateTrackingCode(countryCode);

            UoW.GetRepository<OnlineParticipationRequest>().Insert(onlineParticipationRequest);

            await UoW.SaveAsync();

            string toEmail = (await UoW.GetRepository<User>()
                .GetItemAsycn(e => e.ID == onlineParticipationRequestDto.UserID))
                .Email;

            emailConfig.Details(from: "donotreplybrd@gmail.com",
                to: toEmail,
                fromPassword: "brdworkshop_96##"
                );

            emailConfig.EmailManager.SendMessage("Online Participation form",
                "You have successfully participated in the online form");

            return CreatedAtRoute("DefaultApi", new { id = onlineParticipationRequest.ID }, onlineParticipationRequest);
        }

        // DELETE: api/OnlineParticipationRequests/5
        [ResponseType(typeof(OnlineParticipationRequest))]
        public async Task<IHttpActionResult> DeleteOnlineParticipationRequest(int id)
        {
            OnlineParticipationRequest onlineParticipationRequest = await UoW.GetRepository<OnlineParticipationRequest>().GetItemAsycn(e => e.ID == id);
            if (onlineParticipationRequest == null)
            {
                return NotFound();
            }

            UoW.GetRepository<OnlineParticipationRequest>().Delete(onlineParticipationRequest);
            await UoW.SaveAsync();

            return Ok(onlineParticipationRequest);
        }



        private bool OnlineParticipationRequestExists(int id)
        {
            return UoW.GetRepository<OnlineParticipationRequest>().GetItem(e => e.ID == id) != null;
        }

        private string GenerateTrackingCode(string CountryCode)
        {
            return CountryCode + " : " + DateTime.UtcNow
                .ToString("yyyy/MM/dd : HH:mm:ss");
        }
    }
}
