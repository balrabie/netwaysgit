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

    public class NationalityController : ApiController
    {
        private UnitOfWork UoW = null;
        private IMapper EntityToDtoIMapper = null;
        private IMapper DtoToEntityIMapper = null;

        public NationalityController()
        {
            UoW = new UnitOfWork();
            InitializeMapping();

        }

        public NationalityController(UnitOfWork uoW)
        {
            UoW = uoW;
            InitializeMapping();
        }

        private void InitializeMapping()
        {
            EntityToDtoIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<Nationality, NationalityDto>())
                .CreateMapper();
            DtoToEntityIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<NationalityDto, Nationality>())
                .CreateMapper();
        }

        // GET: api/Nationalitys
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            var items = UoW.GetRepository<Nationality>().GetAll().ToList();
            var DTO = EntityToDtoIMapper.Map<List<Nationality>, List<NationalityDto>>(items).ToList();

            return Ok(DTO);
        }

        // GET: api/Nationalitys/5
        [ResponseType(typeof(Nationality))]
        public async Task<IHttpActionResult> GetNationality(int id)
        {
            Nationality item = await UoW.GetRepository<Nationality>().GetItemAsycn(e => e.ID == id);
            if (item == null)
            {
                return NotFound();
            }

            var DTO = EntityToDtoIMapper.Map<Nationality, NationalityDto>(item);
            return Ok(DTO);
        }

        // PUT: api/Nationalitys/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutNationality(int id, NationalityDto nationalityDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != nationalityDto.ID)
            {
                return BadRequest();
            }
            var nationality = DtoToEntityIMapper.Map<NationalityDto, Nationality>(nationalityDto); ////
            UoW.GetRepository<Nationality>().ModifyEntityState(nationality);

            try
            {
                await UoW.SaveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NationalityExists(id))
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

        // POST: api/Nationalitys
        [ResponseType(typeof(Nationality))]
        public async Task<IHttpActionResult> PostNationality(NationalityDto nationalityDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var nationality = DtoToEntityIMapper.Map<NationalityDto, Nationality>(nationalityDto); ////
            UoW.GetRepository<Nationality>().Insert(nationality);
            await UoW.SaveAsync();

            return CreatedAtRoute("DefaultApi", new { id = nationality.ID }, nationality);
        }

        // DELETE: api/Nationalitys/5
        [ResponseType(typeof(Nationality))]
        public async Task<IHttpActionResult> DeleteNationality(int id)
        {
            Nationality nationality = await UoW.GetRepository<Nationality>().GetItemAsycn(e => e.ID == id);
            if (nationality == null)
            {
                return NotFound();
            }

            UoW.GetRepository<Nationality>().Delete(nationality);
            await UoW.SaveAsync();

            return Ok(nationality);
        }



        private bool NationalityExists(int id)
        {
            return UoW.GetRepository<Nationality>().GetItem(e => e.ID == id) != null;
        }
    }
}
