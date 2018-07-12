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

    public class SocialMediaAccountController : ApiController
    {
        private UnitOfWork UoW = null;
        private IMapper EntityToDtoIMapper = null;
        private IMapper DtoToEntityIMapper = null;

        public SocialMediaAccountController()
        {
            UoW = new UnitOfWork();
            InitializeMapping();

        }

        public SocialMediaAccountController(UnitOfWork uoW)
        {
            UoW = uoW;
            InitializeMapping();
        }

        private void InitializeMapping()
        {
            EntityToDtoIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<SocialMediaAccount, SocialMediaAccountDto>())
                .CreateMapper();
            DtoToEntityIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<SocialMediaAccountDto, SocialMediaAccount>())
                .CreateMapper();
        }

        // GET: api/SocialMediaAccounts
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            var items = UoW.GetRepository<SocialMediaAccount>().GetAll().ToList();
            var DTO = EntityToDtoIMapper.Map<List<SocialMediaAccount>, List<SocialMediaAccountDto>>(items).ToList();

            return Ok(DTO);
        }

        // GET: api/SocialMediaAccounts/5
        [ResponseType(typeof(SocialMediaAccount))]
        public async Task<IHttpActionResult> GetSocialMediaAccount(int id)
        {
            SocialMediaAccount item = await UoW.GetRepository<SocialMediaAccount>().GetItemAsycn(e => e.ID == id);
            if (item == null)
            {
                return NotFound();
            }

            var DTO = EntityToDtoIMapper.Map<SocialMediaAccount, SocialMediaAccountDto>(item);
            return Ok(DTO);
        }

        // PUT: api/SocialMediaAccounts/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSocialMediaAccount(int id, SocialMediaAccountDto socialMediaAccountDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != socialMediaAccountDto.ID)
            {
                return BadRequest();
            }
            var socialMediaAccount = DtoToEntityIMapper.Map<SocialMediaAccountDto, SocialMediaAccount>(socialMediaAccountDto); ////
            UoW.GetRepository<SocialMediaAccount>().ModifyEntityState(socialMediaAccount);

            try
            {
                await UoW.SaveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SocialMediaAccountExists(id))
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

        // POST: api/SocialMediaAccounts
        [ResponseType(typeof(SocialMediaAccount))]
        public async Task<IHttpActionResult> PostSocialMediaAccount(SocialMediaAccountDto socialMediaAccountDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var socialMediaAccount = DtoToEntityIMapper.Map<SocialMediaAccountDto, SocialMediaAccount>(socialMediaAccountDto); ////
            UoW.GetRepository<SocialMediaAccount>().Insert(socialMediaAccount);
            await UoW.SaveAsync();

            return CreatedAtRoute("DefaultApi", new { id = socialMediaAccount.ID }, socialMediaAccount);
        }

        // DELETE: api/SocialMediaAccounts/5
        [ResponseType(typeof(SocialMediaAccount))]
        public async Task<IHttpActionResult> DeleteSocialMediaAccount(int id)
        {
            SocialMediaAccount socialMediaAccount = await UoW.GetRepository<SocialMediaAccount>().GetItemAsycn(e => e.ID == id);
            if (socialMediaAccount == null)
            {
                return NotFound();
            }

            UoW.GetRepository<SocialMediaAccount>().Delete(socialMediaAccount);
            await UoW.SaveAsync();

            return Ok(socialMediaAccount);
        }



        private bool SocialMediaAccountExists(int id)
        {
            return UoW.GetRepository<SocialMediaAccount>().GetItem(e => e.ID == id) != null;
        }
    }
}
