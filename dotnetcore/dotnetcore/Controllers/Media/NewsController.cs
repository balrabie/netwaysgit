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
    public class NewsController : ControllerBase
    {
        private UnitOfWork uoW = null;
        private IRepository<News> repository = null;

        private IMapper EntityToDtoIMapper = null;
        private IMapper DtoToEntityIMapper = null;

        private void InitializeMapping()
        {
            EntityToDtoIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<News, NewsDto>())
                .CreateMapper();
            DtoToEntityIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<NewsDto, News>())
                .CreateMapper();
        }

        public NewsController()
        {
            uoW = new UnitOfWork();
            repository = uoW.GetRepository<News>();
            InitializeMapping();
        }

        // GET: api/News
        [HttpGet]
        public IEnumerable<NewsDto> Index()
        {
            List<NewsDto> newsDto = EntityToDtoIMapper
                .Map<List<News>, List<NewsDto>>(repository.GetAll().ToList())
                .ToList();

            return newsDto;
        }

        // GET: api/News/5
        [HttpGet("{id}")]
        public async Task<ActionResult<NewsDto>> GetNews([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var news = await repository.GetAsync(a => a.ID == id);

            if (news == null)
            {
                return NotFound();
            }

            NewsDto newsDto = EntityToDtoIMapper.Map<News, NewsDto>(news);

            return Ok(newsDto);
        }

        // PUT: api/News/5
        [HttpPut("{id}")]
        public async Task<ActionResult<NewsDto>> PutNews([FromRoute] int id, [FromBody] NewsDto newsDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != newsDto.ID)
            {
                return BadRequest();
            }

            News news = DtoToEntityIMapper.Map<NewsDto, News>(newsDto);

            repository.ModifyEntryState(news, EntityState.Modified);

            try
            {
                await uoW.SaveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NewsExists(id))
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

        // POST: api/Newss
        [HttpPost]
        public async Task<ActionResult<NewsDto>> PostNews([FromBody] NewsDto newsDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            News news = DtoToEntityIMapper.Map<NewsDto, News>(newsDto);

            repository.Add(news);
            await uoW.SaveAsync();

            return CreatedAtAction("GetNews", new { id = news.ID }, newsDto);
        }

        // DELETE: api/Newss/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<NewsDto>> DeleteNews([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            News news = await repository.GetAsync(a => a.ID == id);

            if (news == null)
            {
                return NotFound();
            }

            repository.Delete(news);
            await uoW.SaveAsync();

            NewsDto newsDto = EntityToDtoIMapper.Map<News, NewsDto>(news);

            return Ok(newsDto);
        }

        private bool NewsExists(int id)
        {
            return repository.Get(a => a.ID == id) != null;
        }
    }
}