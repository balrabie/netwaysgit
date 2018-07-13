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
    public class PhotoController : ControllerBase
    {
        private UnitOfWork uoW = null;
        private IRepository<Photo> repository = null;

        private IMapper EntityToDtoIMapper = null;
        private IMapper DtoToEntityIMapper = null;

        private void InitializeMapping()
        {
            EntityToDtoIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<Photo, PhotoDto>())
                .CreateMapper();
            DtoToEntityIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<PhotoDto, Photo>())
                .CreateMapper();
        }

        public PhotoController()
        {
            uoW = new UnitOfWork();
            repository = uoW.GetRepository<Photo>();
            InitializeMapping();
        }

        // GET: api/Photo
        [HttpGet]
        public IEnumerable<PhotoDto> Index()
        {
            List<PhotoDto> photoDto = EntityToDtoIMapper
                .Map<List<Photo>, List<PhotoDto>>(repository.GetAll().ToList())
                .ToList();

            return photoDto;
        }

        // GET: api/Photo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PhotoDto>> GetPhoto([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var photo = await repository.GetAsync(a => a.ID == id);

            if (photo == null)
            {
                return NotFound();
            }

            PhotoDto photoDto = EntityToDtoIMapper.Map<Photo, PhotoDto>(photo);

            return Ok(photoDto);
        }

        // PUT: api/Photo/5
        [HttpPut("{id}")]
        public async Task<ActionResult<PhotoDto>> PutPhoto([FromRoute] int id, [FromBody] PhotoDto photoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != photoDto.ID)
            {
                return BadRequest();
            }

            Photo photo = DtoToEntityIMapper.Map<PhotoDto, Photo>(photoDto);

            repository.ModifyEntryState(photo, EntityState.Modified);

            try
            {
                await uoW.SaveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PhotoExists(id))
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

        // POST: api/Photos
        [HttpPost]
        public async Task<ActionResult<PhotoDto>> PostPhoto([FromBody] PhotoDto photoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Photo photo = DtoToEntityIMapper.Map<PhotoDto, Photo>(photoDto);

            repository.Add(photo);
            await uoW.SaveAsync();

            return CreatedAtAction("GetPhoto", new { id = photo.ID }, photoDto);
        }

        // DELETE: api/Photos/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<PhotoDto>> DeletePhoto([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Photo photo = await repository.GetAsync(a => a.ID == id);

            if (photo == null)
            {
                return NotFound();
            }

            repository.Delete(photo);
            await uoW.SaveAsync();

            PhotoDto photoDto = EntityToDtoIMapper.Map<Photo, PhotoDto>(photo);

            return Ok(photoDto);
        }

        private bool PhotoExists(int id)
        {
            return repository.Get(a => a.ID == id) != null;
        }
    }
}