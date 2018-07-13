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
    public class SocialMediaAccountController : ControllerBase
    {
        private UnitOfWork uoW = null;
        private IRepository<SocialMediaAccount> repository = null;

        private IMapper EntityToDtoIMapper = null;
        private IMapper DtoToEntityIMapper = null;

        private void InitializeMapping()
        {
            EntityToDtoIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<SocialMediaAccount, SocialMediaAccountDto>())
                .CreateMapper();
            DtoToEntityIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<SocialMediaAccountDto, SocialMediaAccount>())
                .CreateMapper();
        }

        public SocialMediaAccountController()
        {
            uoW = new UnitOfWork();
            repository = uoW.GetRepository<SocialMediaAccount>();
            InitializeMapping();
        }

        // GET: api/SocialMediaAccount
        [HttpGet]
        public IEnumerable<SocialMediaAccountDto> Index()
        {
            List<SocialMediaAccountDto> socialMediaAccountDto = EntityToDtoIMapper
                .Map<List<SocialMediaAccount>, List<SocialMediaAccountDto>>(repository.GetAll().ToList())
                .ToList();

            return socialMediaAccountDto;
        }

        // GET: api/SocialMediaAccount/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SocialMediaAccountDto>> GetSocialMediaAccount([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var socialMediaAccount = await repository.GetAsync(a => a.ID == id);

            if (socialMediaAccount == null)
            {
                return NotFound();
            }

            SocialMediaAccountDto socialMediaAccountDto = EntityToDtoIMapper.Map<SocialMediaAccount, SocialMediaAccountDto>(socialMediaAccount);

            return Ok(socialMediaAccountDto);
        }

        // PUT: api/SocialMediaAccount/5
        [HttpPut("{id}")]
        public async Task<ActionResult<SocialMediaAccountDto>> PutSocialMediaAccount([FromRoute] int id, [FromBody] SocialMediaAccountDto socialMediaAccountDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != socialMediaAccountDto.ID)
            {
                return BadRequest();
            }

            SocialMediaAccount socialMediaAccount = DtoToEntityIMapper.Map<SocialMediaAccountDto, SocialMediaAccount>(socialMediaAccountDto);

            repository.ModifyEntryState(socialMediaAccount, EntityState.Modified);

            try
            {
                await uoW.SaveAsync();
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

            return NoContent();
        }

        // POST: api/SocialMediaAccounts
        [HttpPost]
        public async Task<ActionResult<SocialMediaAccountDto>> PostSocialMediaAccount([FromBody] SocialMediaAccountDto socialMediaAccountDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            SocialMediaAccount socialMediaAccount = DtoToEntityIMapper.Map<SocialMediaAccountDto, SocialMediaAccount>(socialMediaAccountDto);

            repository.Add(socialMediaAccount);
            await uoW.SaveAsync();

            return CreatedAtAction("GetSocialMediaAccount", new { id = socialMediaAccount.ID }, socialMediaAccountDto);
        }

        // DELETE: api/SocialMediaAccounts/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<SocialMediaAccountDto>> DeleteSocialMediaAccount([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            SocialMediaAccount socialMediaAccount = await repository.GetAsync(a => a.ID == id);

            if (socialMediaAccount == null)
            {
                return NotFound();
            }

            repository.Delete(socialMediaAccount);
            await uoW.SaveAsync();

            SocialMediaAccountDto socialMediaAccountDto = EntityToDtoIMapper.Map<SocialMediaAccount, SocialMediaAccountDto>(socialMediaAccount);

            return Ok(socialMediaAccountDto);
        }

        private bool SocialMediaAccountExists(int id)
        {
            return repository.Get(a => a.ID == id) != null;
        }
    }
}