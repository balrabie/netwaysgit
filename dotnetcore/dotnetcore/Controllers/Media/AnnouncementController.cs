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
    public class AnnouncementController : ControllerBase
    {
        private UnitOfWork uoW = null;
        private IRepository<Announcement> repository = null;

        private IMapper EntityToDtoIMapper = null;
        private IMapper DtoToEntityIMapper = null;

        private void InitializeMapping()
        {
            EntityToDtoIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<Announcement, AnnouncementDto>())
                .CreateMapper();
            DtoToEntityIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<AnnouncementDto, Announcement>())
                .CreateMapper();
        }

        public AnnouncementController()
        {
            uoW = new UnitOfWork();
            repository = uoW.GetRepository<Announcement>();
            InitializeMapping();
        }

        // GET: api/Announcement
        [HttpGet]
        public IEnumerable<AnnouncementDto> Index()
        {
            List<AnnouncementDto> announcementDto = EntityToDtoIMapper
                .Map<List<Announcement>, List<AnnouncementDto>>(repository.GetAll().ToList())
                .ToList();

            return announcementDto;
        }

        // GET: api/Announcement/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AnnouncementDto>> GetAnnouncement([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var announcement = await repository.GetAsync(a => a.ID == id);

            if (announcement == null)
            {
                return NotFound();
            }

            AnnouncementDto announcementDto = EntityToDtoIMapper.Map<Announcement, AnnouncementDto>(announcement);

            return Ok(announcementDto);
        }

        // PUT: api/Announcement/5
        [HttpPut("{id}")]
        public async Task<ActionResult<AnnouncementDto>> PutAnnouncement([FromRoute] int id, [FromBody] AnnouncementDto announcementDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != announcementDto.ID)
            {
                return BadRequest();
            }

            Announcement announcement = DtoToEntityIMapper.Map<AnnouncementDto, Announcement>(announcementDto);

            repository.ModifyEntryState(announcement, EntityState.Modified);

            try
            {
                await uoW.SaveAsync();
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

            return NoContent();
        }

        // POST: api/Announcements
        [HttpPost]
        public async Task<ActionResult<AnnouncementDto>> PostAnnouncement([FromBody] AnnouncementDto announcementDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Announcement announcement = DtoToEntityIMapper.Map<AnnouncementDto, Announcement>(announcementDto);

            repository.Add(announcement);
            await uoW.SaveAsync();

            return CreatedAtAction("GetAnnouncement", new { id = announcement.ID }, announcementDto);
        }

        // DELETE: api/Announcements/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<AnnouncementDto>> DeleteAnnouncement([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Announcement announcement = await repository.GetAsync(a => a.ID == id);

            if (announcement == null)
            {
                return NotFound();
            }

            repository.Delete(announcement);
            await uoW.SaveAsync();

            AnnouncementDto announcementDto = EntityToDtoIMapper.Map<Announcement, AnnouncementDto>(announcement);

            return Ok(announcementDto);
        }

        private bool AnnouncementExists(int id)
        {
            return repository.Get(a => a.ID == id) != null;
        }
    }
}