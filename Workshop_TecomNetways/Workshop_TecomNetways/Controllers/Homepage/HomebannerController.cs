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

    public class HomebannerController : ApiController
    {
        private UnitOfWork UoW = null;
        private IMapper EntityToDtoIMapper = null;
        private IMapper DtoToEntityIMapper = null;

        public HomebannerController()
        {
            UoW = new UnitOfWork();
            InitializeMapping();

        }

        public HomebannerController(UnitOfWork uoW)
        {
            UoW = uoW;
            InitializeMapping();
        }

        private void InitializeMapping()
        {
            EntityToDtoIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<Homebanner, HomebannerDto>())
                .CreateMapper();
            DtoToEntityIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<HomebannerDto, Homebanner>())
                .CreateMapper();
        }

        // GET: api/Homebanners
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            var items = UoW.GetRepository<Homebanner>().GetAll().ToList();
            var DTO = EntityToDtoIMapper.Map<List<Homebanner>, List<HomebannerDto>>(items).ToList();

            return Ok(DTO);
        }

        // GET: api/Homebanners/5
        [ResponseType(typeof(Homebanner))]
        public async Task<IHttpActionResult> GetHomebanner(int id)
        {
            Homebanner item = await UoW.GetRepository<Homebanner>().GetItemAsycn(e => e.ID == id);
            if (item == null)
            {
                return NotFound();
            }

            var DTO = EntityToDtoIMapper.Map<Homebanner, HomebannerDto>(item);
            return Ok(DTO);
        }

        // PUT: api/Homebanners/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutHomebanner(int id, HomebannerDto homebannerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != homebannerDto.ID)
            {
                return BadRequest();
            }
            var homebanner = DtoToEntityIMapper.Map<HomebannerDto, Homebanner>(homebannerDto); ////
            UoW.GetRepository<Homebanner>().ModifyEntityState(homebanner);

            try
            {
                await UoW.SaveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HomebannerExists(id))
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

        // POST: api/Homebanners
        [ResponseType(typeof(Homebanner))]
        public async Task<IHttpActionResult> PostHomebanner(HomebannerDto homebannerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var homebanner = DtoToEntityIMapper.Map<HomebannerDto, Homebanner>(homebannerDto); ////
            UoW.GetRepository<Homebanner>().Insert(homebanner);
            await UoW.SaveAsync();

            return CreatedAtRoute("DefaultApi", new { id = homebanner.ID }, homebanner);
        }

        // DELETE: api/Homebanners/5
        [ResponseType(typeof(Homebanner))]
        public async Task<IHttpActionResult> DeleteHomebanner(int id)
        {
            Homebanner homebanner = await UoW.GetRepository<Homebanner>().GetItemAsycn(e => e.ID == id);
            if (homebanner == null)
            {
                return NotFound();
            }

            UoW.GetRepository<Homebanner>().Delete(homebanner);
            await UoW.SaveAsync();

            return Ok(homebanner);
        }



        private bool HomebannerExists(int id)
        {
            return UoW.GetRepository<Homebanner>().GetItem(e => e.ID == id) != null;
        }
    }
}
