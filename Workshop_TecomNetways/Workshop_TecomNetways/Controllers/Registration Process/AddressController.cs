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

    public class AddressController : ApiController
    {
        private UnitOfWork UoW = null;
        private IMapper EntityToDtoIMapper = null;
        private IMapper DtoToEntityIMapper = null;

        private void InitializeMapping()
        {
            EntityToDtoIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<Address, AddressDto>())
                .CreateMapper();
            DtoToEntityIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<AddressDto, Address>())
                .CreateMapper();
        }

        public AddressController()
        {
            UoW = new UnitOfWork();
            InitializeMapping();

        }

        public AddressController(UnitOfWork uoW)
        {
            UoW = uoW;
            InitializeMapping();
        }


        // GET: api/Addresss
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            var items = UoW.GetRepository<Address>().GetAll().ToList();
            var DTO = EntityToDtoIMapper.Map<List<Address>, List<AddressDto>>(items).ToList();

            return Ok(DTO);
        }

        // GET: api/Addresss/5
        [ResponseType(typeof(Address))]
        public async Task<IHttpActionResult> GetAddress(int id)
        {
            Address item = await UoW.GetRepository<Address>().GetItemAsycn(e => e.ID == id);
            if (item == null)
            {
                return NotFound();
            }

            var DTO = EntityToDtoIMapper.Map<Address, AddressDto>(item);
            return Ok(DTO);
        }

        // PUT: api/Addresss/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutAddress(int id, AddressDto addressDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != addressDto.ID)
            {
                return BadRequest();
            }
            var address = DtoToEntityIMapper.Map<AddressDto, Address>(addressDto); ////
            UoW.GetRepository<Address>().ModifyEntityState(address);

            try
            {
                await UoW.SaveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AddressExists(id))
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

        // POST: api/Addresss
        [ResponseType(typeof(Address))]
        public async Task<IHttpActionResult> PostAddress(AddressDto addressDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var address = DtoToEntityIMapper.Map<AddressDto, Address>(addressDto); ////
            UoW.GetRepository<Address>().Insert(address);
            await UoW.SaveAsync();

            return CreatedAtRoute("DefaultApi", new { id = address.ID }, address);
        }

        // DELETE: api/Addresss/5
        [ResponseType(typeof(Address))]
        public async Task<IHttpActionResult> DeleteAddress(int id)
        {
            Address address = await UoW.GetRepository<Address>().GetItemAsycn(e => e.ID == id);
            if (address == null)
            {
                return NotFound();
            }

            UoW.GetRepository<Address>().Delete(address);
            await UoW.SaveAsync();

            return Ok(address);
        }



        private bool AddressExists(int id)
        {
            return UoW.GetRepository<Address>().GetItem(e => e.ID == id) != null;
        }
    }
}
