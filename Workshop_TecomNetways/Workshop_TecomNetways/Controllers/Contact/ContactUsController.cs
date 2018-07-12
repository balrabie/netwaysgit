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

    public class ContactUsController : ApiController
    {
        private UnitOfWork UoW = null;
        private IMapper EntityToDtoIMapper = null;
        private IMapper DtoToEntityIMapper = null;

        public ContactUsController()
        {
            UoW = new UnitOfWork();
            InitializeMapping();

        }

        public ContactUsController(UnitOfWork uoW)
        {
            UoW = uoW;
            InitializeMapping();
        }

        private void InitializeMapping()
        {
            EntityToDtoIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<ContactUs, ContactUsDto>())
                .CreateMapper();
            DtoToEntityIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<ContactUsDto, ContactUs>())
                .CreateMapper();
        }

        // GET: api/ContactUss
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            var items = UoW.GetRepository<ContactUs>().GetAll().ToList();
            var DTO = EntityToDtoIMapper.Map<List<ContactUs>, List<ContactUsDto>>(items).ToList();

            return Ok(DTO);
        }

        // GET: api/ContactUss/5
        [ResponseType(typeof(ContactUs))]
        public async Task<IHttpActionResult> GetContactUs(int id)
        {
            ContactUs item = await UoW.GetRepository<ContactUs>().GetItemAsycn(e => e.ID == id);
            if (item == null)
            {
                return NotFound();
            }

            var DTO = EntityToDtoIMapper.Map<ContactUs, ContactUsDto>(item);
            return Ok(DTO);
        }

        // PUT: api/ContactUss/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutContactUs(int id, ContactUsDto contactUsDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != contactUsDto.ID)
            {
                return BadRequest();
            }
            var contactUs = DtoToEntityIMapper.Map<ContactUsDto, ContactUs>(contactUsDto); ////
            UoW.GetRepository<ContactUs>().ModifyEntityState(contactUs);

            try
            {
                await UoW.SaveAsync();
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

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/ContactUss
        [ResponseType(typeof(ContactUs))]
        public async Task<IHttpActionResult> PostContactUs(ContactUsDto contactUsDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var contactUs = DtoToEntityIMapper.Map<ContactUsDto, ContactUs>(contactUsDto); ////
            UoW.GetRepository<ContactUs>().Insert(contactUs);
            await UoW.SaveAsync();

            return CreatedAtRoute("DefaultApi", new { id = contactUs.ID }, contactUs);
        }

        // DELETE: api/ContactUss/5
        [ResponseType(typeof(ContactUs))]
        public async Task<IHttpActionResult> DeleteContactUs(int id)
        {
            ContactUs contactUs = await UoW.GetRepository<ContactUs>().GetItemAsycn(e => e.ID == id);
            if (contactUs == null)
            {
                return NotFound();
            }

            UoW.GetRepository<ContactUs>().Delete(contactUs);
            await UoW.SaveAsync();

            return Ok(contactUs);
        }



        private bool ContactUsExists(int id)
        {
            return UoW.GetRepository<ContactUs>().GetItem(e => e.ID == id) != null;
        }
    }
}
