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
    public class ContactUsController : ControllerBase
    {
        private UnitOfWork uoW = null;
        private IRepository<ContactUs> repository = null;

        private IMapper EntityToDtoIMapper = null;
        private IMapper DtoToEntityIMapper = null;

        private void InitializeMapping()
        {
            EntityToDtoIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<ContactUs, ContactUsDto>())
                .CreateMapper();
            DtoToEntityIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<ContactUsDto, ContactUs>())
                .CreateMapper();
        }

        public ContactUsController()
        {
            uoW = new UnitOfWork();
            repository = uoW.GetRepository<ContactUs>();
            InitializeMapping();
        }

        // GET: api/ContactUs
        [HttpGet]
        public IEnumerable<ContactUsDto> Index()
        {
            List<ContactUsDto> contactUsDto = EntityToDtoIMapper
                .Map<List<ContactUs>, List<ContactUsDto>>(repository.GetAll().ToList())
                .ToList();

            return contactUsDto;
        }

        // GET: api/ContactUs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ContactUsDto>> GetContactUs([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var contactUs = await repository.GetAsync(a => a.ID == id);

            if (contactUs == null)
            {
                return NotFound();
            }

            ContactUsDto contactUsDto = EntityToDtoIMapper.Map<ContactUs, ContactUsDto>(contactUs);

            return Ok(contactUsDto);
        }

        // PUT: api/ContactUs/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ContactUsDto>> PutContactUs([FromRoute] int id, [FromBody] ContactUsDto contactUsDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != contactUsDto.ID)
            {
                return BadRequest();
            }

            ContactUs contactUs = DtoToEntityIMapper.Map<ContactUsDto, ContactUs>(contactUsDto);

            repository.ModifyEntryState(contactUs, EntityState.Modified);

            try
            {
                await uoW.SaveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactUsExists(id))
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

        // POST: api/ContactUss
        [HttpPost]
        public async Task<ActionResult<ContactUsDto>> PostContactUs([FromBody] ContactUsDto contactUsDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ContactUs contactUs = DtoToEntityIMapper.Map<ContactUsDto, ContactUs>(contactUsDto);

            repository.Add(contactUs);
            await uoW.SaveAsync();

            return CreatedAtAction("GetContactUs", new { id = contactUs.ID }, contactUsDto);
        }

        // DELETE: api/ContactUss/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ContactUsDto>> DeleteContactUs([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ContactUs contactUs = await repository.GetAsync(a => a.ID == id);

            if (contactUs == null)
            {
                return NotFound();
            }

            repository.Delete(contactUs);
            await uoW.SaveAsync();

            ContactUsDto contactUsDto = EntityToDtoIMapper.Map<ContactUs, ContactUsDto>(contactUs);

            return Ok(contactUsDto);
        }

        private bool ContactUsExists(int id)
        {
            return repository.Get(a => a.ID == id) != null;
        }
    }
}