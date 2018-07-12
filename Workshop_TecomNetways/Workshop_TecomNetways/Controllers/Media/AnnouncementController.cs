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

    public class AnnouncementController : ApiController
    {
        private UnitOfWork UoW = null;
        private IMapper EntityToDtoIMapper = null;
        private IMapper DtoToEntityIMapper = null;

        public AnnouncementController()
        {
            UoW = new UnitOfWork();
            InitializeMapping();

        }

        public AnnouncementController(UnitOfWork uoW)
        {
            UoW = uoW;
            InitializeMapping();
        }

        private void InitializeMapping()
        {
            EntityToDtoIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<Announcement, AnnouncementDto>())
                .CreateMapper();
            DtoToEntityIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<AnnouncementDto, Announcement>())
                .CreateMapper();
        }

        // GET: api/Announcements
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            var items = UoW.GetRepository<Announcement>().GetAll().ToList();
            var DTO = EntityToDtoIMapper.Map<List<Announcement>, List<AnnouncementDto>>(items).ToList();

            return Ok(DTO);
        }

        // GET: api/Announcements/5
        [ResponseType(typeof(Announcement))]
        public async Task<IHttpActionResult> GetAnnouncement(int id)
        {
            Announcement item = await UoW.GetRepository<Announcement>().GetItemAsycn(e => e.ID == id);
            if (item == null)
            {
                return NotFound();
            }

            var DTO = EntityToDtoIMapper.Map<Announcement, AnnouncementDto>(item);
            return Ok(DTO);
        }

        // PUT: api/Announcements/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutAnnouncement(int id, AnnouncementDto announcementDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != announcementDto.ID)
            {
                return BadRequest();
            }
            var announcement = DtoToEntityIMapper.Map<AnnouncementDto, Announcement>(announcementDto); ////
            UoW.GetRepository<Announcement>().ModifyEntityState(announcement);

            try
            {
                await UoW.SaveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnnouncementExists(id))
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

        // POST: api/Announcements
        [ResponseType(typeof(Announcement))]
        public async Task<IHttpActionResult> PostAnnouncement(AnnouncementDto announcementDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var announcement = DtoToEntityIMapper.Map<AnnouncementDto, Announcement>(announcementDto); ////
            UoW.GetRepository<Announcement>().Insert(announcement);
            await UoW.SaveAsync();

            return CreatedAtRoute("DefaultApi", new { id = announcement.ID }, announcement);
        }

        // DELETE: api/Announcements/5
        [ResponseType(typeof(Announcement))]
        public async Task<IHttpActionResult> DeleteAnnouncement(int id)
        {
            Announcement announcement = await UoW.GetRepository<Announcement>().GetItemAsycn(e => e.ID == id);
            if (announcement == null)
            {
                return NotFound();
            }

            UoW.GetRepository<Announcement>().Delete(announcement);
            await UoW.SaveAsync();

            return Ok(announcement);
        }



        private bool AnnouncementExists(int id)
        {
            return UoW.GetRepository<Announcement>().GetItem(e => e.ID == id) != null;
        }
    }
}
