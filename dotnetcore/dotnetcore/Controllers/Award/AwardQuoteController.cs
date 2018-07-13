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
    public class AwardQuoteController : ControllerBase
    {
        private UnitOfWork uoW = null;
        private IRepository<AwardQuote> repository = null;

        private IMapper EntityToDtoIMapper = null;
        private IMapper DtoToEntityIMapper = null;

        private void InitializeMapping()
        {
            EntityToDtoIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<AwardQuote, AwardQuoteDto>())
                .CreateMapper();
            DtoToEntityIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<AwardQuoteDto, AwardQuote>())
                .CreateMapper();
        }

        public AwardQuoteController()
        {
            uoW = new UnitOfWork();
            repository = uoW.GetRepository<AwardQuote>();
            InitializeMapping();
        }

        // GET: api/AwardQuote
        [HttpGet]
        public IEnumerable<AwardQuoteDto> Index()
        {
            List<AwardQuoteDto> awardQuoteDto = EntityToDtoIMapper
                .Map<List<AwardQuote>, List<AwardQuoteDto>>(repository.GetAll().ToList())
                .ToList();

            return awardQuoteDto;
        }

        // GET: api/AwardQuote/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AwardQuoteDto>> GetAwardQuote([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var awardQuote = await repository.GetAsync(a => a.ID == id);

            if (awardQuote == null)
            {
                return NotFound();
            }

            AwardQuoteDto awardQuoteDto = EntityToDtoIMapper.Map<AwardQuote, AwardQuoteDto>(awardQuote);

            return Ok(awardQuoteDto);
        }

        // PUT: api/AwardQuote/5
        [HttpPut("{id}")]
        public async Task<ActionResult<AwardQuoteDto>> PutAwardQuote([FromRoute] int id, [FromBody] AwardQuoteDto awardQuoteDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != awardQuoteDto.ID)
            {
                return BadRequest();
            }

            AwardQuote awardQuote = DtoToEntityIMapper.Map<AwardQuoteDto, AwardQuote>(awardQuoteDto);

            repository.ModifyEntryState(awardQuote, EntityState.Modified);

            try
            {
                await uoW.SaveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AwardQuoteExists(id))
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

        // POST: api/AwardQuotes
        [HttpPost]
        public async Task<ActionResult<AwardQuoteDto>> PostAwardQuote([FromBody] AwardQuoteDto awardQuoteDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            AwardQuote awardQuote = DtoToEntityIMapper.Map<AwardQuoteDto, AwardQuote>(awardQuoteDto);

            repository.Add(awardQuote);
            await uoW.SaveAsync();

            return CreatedAtAction("GetAwardQuote", new { id = awardQuote.ID }, awardQuoteDto);
        }

        // DELETE: api/AwardQuotes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<AwardQuoteDto>> DeleteAwardQuote([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            AwardQuote awardQuote = await repository.GetAsync(a => a.ID == id);

            if (awardQuote == null)
            {
                return NotFound();
            }

            repository.Delete(awardQuote);
            await uoW.SaveAsync();

            AwardQuoteDto awardQuoteDto = EntityToDtoIMapper.Map<AwardQuote, AwardQuoteDto>(awardQuote);

            return Ok(awardQuoteDto);
        }

        private bool AwardQuoteExists(int id)
        {
            return repository.Get(a => a.ID == id) != null;
        }
    }
}