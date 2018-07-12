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

    public class CountryController : ApiController
    {
        private UnitOfWork UoW = null;
        private IMapper EntityToDtoIMapper = null;
        private IMapper DtoToEntityIMapper = null;

        public CountryController()
        {
            UoW = new UnitOfWork();
            InitializeMapping();

        }

        public CountryController(UnitOfWork uoW)
        {
            UoW = uoW;
            InitializeMapping();
        }

        private void InitializeMapping()
        {
            EntityToDtoIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<Country, CountryDto>())
                .CreateMapper();
            DtoToEntityIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<CountryDto, Country>())
                .CreateMapper();
        }

        // GET: api/Countrys
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            var items = UoW.GetRepository<Country>().GetAll().ToList();
            var DTO = EntityToDtoIMapper.Map<List<Country>, List<CountryDto>>(items).ToList();

            return Ok(DTO);
        }

        // GET: api/Countrys/5
        [ResponseType(typeof(Country))]
        public async Task<IHttpActionResult> GetCountry(int id)
        {
            Country item = await UoW.GetRepository<Country>().GetItemAsycn(e => e.ID == id);
            if (item == null)
            {
                return NotFound();
            }

            var DTO = EntityToDtoIMapper.Map<Country, CountryDto>(item);
            return Ok(DTO);
        }

        // PUT: api/Countrys/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutCountry(int id, CountryDto countryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != countryDto.ID)
            {
                return BadRequest();
            }
            var country = DtoToEntityIMapper.Map<CountryDto, Country>(countryDto); ////
            UoW.GetRepository<Country>().ModifyEntityState(country);

            try
            {
                await UoW.SaveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CountryExists(id))
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

        // POST: api/Countrys
        [ResponseType(typeof(Country))]
        public async Task<IHttpActionResult> PostCountry(CountryDto countryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var country = DtoToEntityIMapper.Map<CountryDto, Country>(countryDto); ////
            UoW.GetRepository<Country>().Insert(country);
            await UoW.SaveAsync();

            return CreatedAtRoute("DefaultApi", new { id = country.ID }, country);
        }

        // DELETE: api/Countrys/5
        [ResponseType(typeof(Country))]
        public async Task<IHttpActionResult> DeleteCountry(int id)
        {
            Country country = await UoW.GetRepository<Country>().GetItemAsycn(e => e.ID == id);
            if (country == null)
            {
                return NotFound();
            }

            UoW.GetRepository<Country>().Delete(country);
            await UoW.SaveAsync();

            return Ok(country);
        }



        private bool CountryExists(int id)
        {
            return UoW.GetRepository<Country>().GetItem(e => e.ID == id) != null;
        }
    }
}
