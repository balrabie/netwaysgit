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
    public class PhotoAlbumController : ControllerBase
    {
        private UnitOfWork uoW = null;
        private IRepository<PhotoAlbum> repository = null;

        private IMapper EntityToDtoIMapper = null;
        private IMapper DtoToEntityIMapper = null;

        private void InitializeMapping()
        {
            EntityToDtoIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<PhotoAlbum, PhotoAlbumDto>())
                .CreateMapper();
            DtoToEntityIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<PhotoAlbumDto, PhotoAlbum>())
                .CreateMapper();
        }

        public PhotoAlbumController()
        {
            uoW = new UnitOfWork();
            repository = uoW.GetRepository<PhotoAlbum>();
            InitializeMapping();
        }

        // GET: api/PhotoAlbum
        [HttpGet]
        public IEnumerable<PhotoAlbumDto> Index()
        {
            List<PhotoAlbumDto> photoAlbumDto = EntityToDtoIMapper
                .Map<List<PhotoAlbum>, List<PhotoAlbumDto>>(repository.GetAll().ToList())
                .ToList();

            return photoAlbumDto;
        }

        // GET: api/PhotoAlbum/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PhotoAlbumDto>> GetPhotoAlbum([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var photoAlbum = await repository.GetAsync(a => a.ID == id);

            if (photoAlbum == null)
            {
                return NotFound();
            }

            PhotoAlbumDto photoAlbumDto = EntityToDtoIMapper.Map<PhotoAlbum, PhotoAlbumDto>(photoAlbum);

            return Ok(photoAlbumDto);
        }

        // PUT: api/PhotoAlbum/5
        [HttpPut("{id}")]
        public async Task<ActionResult<PhotoAlbumDto>> PutPhotoAlbum([FromRoute] int id, [FromBody] PhotoAlbumDto photoAlbumDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != photoAlbumDto.ID)
            {
                return BadRequest();
            }

            PhotoAlbum photoAlbum = DtoToEntityIMapper.Map<PhotoAlbumDto, PhotoAlbum>(photoAlbumDto);

            repository.ModifyEntryState(photoAlbum, EntityState.Modified);

            try
            {
                await uoW.SaveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PhotoAlbumExists(id))
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

        // POST: api/PhotoAlbums
        [HttpPost]
        public async Task<ActionResult<PhotoAlbumDto>> PostPhotoAlbum([FromBody] PhotoAlbumDto photoAlbumDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            PhotoAlbum photoAlbum = DtoToEntityIMapper.Map<PhotoAlbumDto, PhotoAlbum>(photoAlbumDto);

            repository.Add(photoAlbum);
            await uoW.SaveAsync();

            return CreatedAtAction("GetPhotoAlbum", new { id = photoAlbum.ID }, photoAlbumDto);
        }

        // DELETE: api/PhotoAlbums/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<PhotoAlbumDto>> DeletePhotoAlbum([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            PhotoAlbum photoAlbum = await repository.GetAsync(a => a.ID == id);

            if (photoAlbum == null)
            {
                return NotFound();
            }

            repository.Delete(photoAlbum);
            await uoW.SaveAsync();

            PhotoAlbumDto photoAlbumDto = EntityToDtoIMapper.Map<PhotoAlbum, PhotoAlbumDto>(photoAlbum);

            return Ok(photoAlbumDto);
        }

        private bool PhotoAlbumExists(int id)
        {
            return repository.Get(a => a.ID == id) != null;
        }
    }
}