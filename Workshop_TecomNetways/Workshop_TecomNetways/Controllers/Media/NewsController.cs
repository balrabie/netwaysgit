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

    public class NewsController : ApiController
    {
        private UnitOfWork UoW = null;
        private IMapper EntityToDtoIMapper = null;
        private IMapper DtoToEntityIMapper = null;

        public NewsController()
        {
            UoW = new UnitOfWork();
            InitializeMapping();

        }

        public NewsController(UnitOfWork uoW)
        {
            UoW = uoW;
            InitializeMapping();
        }

        private void InitializeMapping()
        {
            EntityToDtoIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<News, NewsDto>())
                .CreateMapper();
            DtoToEntityIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<NewsDto, News>())
                .CreateMapper();
        }

        // GET: api/Newss
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            var items = UoW.GetRepository<News>().GetAll().ToList();
            var DTO = EntityToDtoIMapper.Map<List<News>, List<NewsDto>>(items).ToList();

            return Ok(DTO);
        }

        // GET: api/Newss/5
        [ResponseType(typeof(News))]
        public async Task<IHttpActionResult> GetNews(int id)
        {
            News item = await UoW.GetRepository<News>().GetItemAsycn(e => e.ID == id);
            if (item == null)
            {
                return NotFound();
            }

            var DTO = EntityToDtoIMapper.Map<News, NewsDto>(item);
            return Ok(DTO);
        }

        // PUT: api/Newss/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutNews(int id, NewsDto newsDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != newsDto.ID)
            {
                return BadRequest();
            }
            var news = DtoToEntityIMapper.Map<NewsDto, News>(newsDto); ////
            UoW.GetRepository<News>().ModifyEntityState(news);

            try
            {
                await UoW.SaveAsync();
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

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Newss
        [ResponseType(typeof(News))]
        public async Task<IHttpActionResult> PostNews(NewsDto newsDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var news = DtoToEntityIMapper.Map<NewsDto, News>(newsDto); ////
            UoW.GetRepository<News>().Insert(news);
            await UoW.SaveAsync();

            return CreatedAtRoute("DefaultApi", new { id = news.ID }, news);
        }

        // DELETE: api/Newss/5
        [ResponseType(typeof(News))]
        public async Task<IHttpActionResult> DeleteNews(int id)
        {
            News news = await UoW.GetRepository<News>().GetItemAsycn(e => e.ID == id);
            if (news == null)
            {
                return NotFound();
            }

            UoW.GetRepository<News>().Delete(news);
            await UoW.SaveAsync();

            return Ok(news);
        }



        private bool NewsExists(int id)
        {
            return UoW.GetRepository<News>().GetItem(e => e.ID == id) != null;
        }
    }
}
