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
    public class AddressController : ControllerBase
    {
        private UnitOfWork uoW = null;
        private IRepository<Address> repository = null;

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
            uoW = new UnitOfWork();
            repository = uoW.GetRepository<Address>();
            InitializeMapping();
        }

        // GET: api/Address
        [HttpGet]
        public IEnumerable<AddressDto> Index()
        {
            List<AddressDto> addressDto = EntityToDtoIMapper
                .Map<List<Address>, List<AddressDto>>(repository.GetAll().ToList())
                .ToList();

            return addressDto;
        }

        // GET: api/Address/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AddressDto>> GetAddress([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var address = await repository.GetAsync(a => a.ID == id);

            if (address == null)
            {
                return NotFound();
            }

            AddressDto addressDto = EntityToDtoIMapper.Map<Address, AddressDto>(address);

            return Ok(addressDto);
        }

        // PUT: api/Address/5
        [HttpPut("{id}")]
        public async Task<ActionResult<AddressDto>> PutAddress([FromRoute] int id, [FromBody] AddressDto addressDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != addressDto.ID)
            {
                return BadRequest();
            }

            Address address = DtoToEntityIMapper.Map<AddressDto, Address>(addressDto);

            repository.ModifyEntryState(address, EntityState.Modified);

            try
            {
                await uoW.SaveAsync();
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

            return NoContent();
        }

        // POST: api/Addresss
        [HttpPost]
        public async Task<ActionResult<AddressDto>> PostAddress([FromBody] AddressDto addressDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Address address = DtoToEntityIMapper.Map<AddressDto, Address>(addressDto);

            repository.Add(address);
            await uoW.SaveAsync();

            return CreatedAtAction("GetAddress", new { id = address.ID }, addressDto);
        }

        // DELETE: api/Addresss/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<AddressDto>> DeleteAddress([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Address address = await repository.GetAsync(a => a.ID == id);

            if (address == null)
            {
                return NotFound();
            }

            repository.Delete(address);
            await uoW.SaveAsync();

            AddressDto addressDto = EntityToDtoIMapper.Map<Address, AddressDto>(address);

            return Ok(addressDto);
        }

        private bool AddressExists(int id)
        {
            return repository.Get(a => a.ID == id) != null;
        }
    }
}