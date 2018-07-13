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
    public class VideoAlbumController : ControllerBase
    {
        private UnitOfWork uoW = null;
        private IRepository<VideoAlbum> repository = null;

        private IMapper EntityToDtoIMapper = null;
        private IMapper DtoToEntityIMapper = null;

        private void InitializeMapping()
        {
            EntityToDtoIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<VideoAlbum, VideoAlbumDto>())
                .CreateMapper();
            DtoToEntityIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<VideoAlbumDto, VideoAlbum>())
                .CreateMapper();
        }

        public VideoAlbumController()
        {
            uoW = new UnitOfWork();
            repository = uoW.GetRepository<VideoAlbum>();
            InitializeMapping();
        }

        // GET: api/VideoAlbum
        [HttpGet]
        public IEnumerable<VideoAlbumDto> Index()
        {
            List<VideoAlbumDto> videoAlbumDto = EntityToDtoIMapper
                .Map<List<VideoAlbum>, List<VideoAlbumDto>>(repository.GetAll().ToList())
                .ToList();

            return videoAlbumDto;
        }

        // GET: api/VideoAlbum/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VideoAlbumDto>> GetVideoAlbum([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var videoAlbum = await repository.GetAsync(a => a.ID == id);

            if (videoAlbum == null)
            {
                return NotFound();
            }

            VideoAlbumDto videoAlbumDto = EntityToDtoIMapper.Map<VideoAlbum, VideoAlbumDto>(videoAlbum);

            return Ok(videoAlbumDto);
        }

        // PUT: api/VideoAlbum/5
        [HttpPut("{id}")]
        public async Task<ActionResult<VideoAlbumDto>> PutVideoAlbum([FromRoute] int id, [FromBody] VideoAlbumDto videoAlbumDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != videoAlbumDto.ID)
            {
                return BadRequest();
            }

            VideoAlbum videoAlbum = DtoToEntityIMapper.Map<VideoAlbumDto, VideoAlbum>(videoAlbumDto);

            repository.ModifyEntryState(videoAlbum, EntityState.Modified);

            try
            {
                await uoW.SaveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VideoAlbumExists(id))
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

        // POST: api/VideoAlbums
        [HttpPost]
        public async Task<ActionResult<VideoAlbumDto>> PostVideoAlbum([FromBody] VideoAlbumDto videoAlbumDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            VideoAlbum videoAlbum = DtoToEntityIMapper.Map<VideoAlbumDto, VideoAlbum>(videoAlbumDto);

            repository.Add(videoAlbum);
            await uoW.SaveAsync();

            return CreatedAtAction("GetVideoAlbum", new { id = videoAlbum.ID }, videoAlbumDto);
        }

        // DELETE: api/VideoAlbums/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<VideoAlbumDto>> DeleteVideoAlbum([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            VideoAlbum videoAlbum = await repository.GetAsync(a => a.ID == id);

            if (videoAlbum == null)
            {
                return NotFound();
            }

            repository.Delete(videoAlbum);
            await uoW.SaveAsync();

            VideoAlbumDto videoAlbumDto = EntityToDtoIMapper.Map<VideoAlbum, VideoAlbumDto>(videoAlbum);

            return Ok(videoAlbumDto);
        }

        [HttpPut("{id}/LinkedTo")]
        public async Task<ActionResult<VideoDto>> 
            AssociateWithVideos([FromRoute] int id, [FromBody] int[] videoIDs)
        {
            VideoAlbum videoAlbum = await repository.GetAsync(e => e.ID == id);

            if (videoAlbum == null)
            {
                return BadRequest();
            }

            foreach (int videoID in videoIDs)
            {
                Video video = await uoW.GetRepository<Video>().GetAsync(e => e.ID == id);
                if (video == null)
                {
                    continue;
                }

                var relation = new VideoAlbumVideo()
                {
                    VideoID = videoID,
                    VideoAlbumID = id,
                    Video = video,
                    VideoAlbum = videoAlbum
                };

                if (videoAlbum.Videos == null)
                {
                    videoAlbum.Videos = new List<VideoAlbumVideo>();
                }
                repository.ModifyEntryState(videoAlbum, EntityState.Modified);
                videoAlbum.Videos.Add(relation);
            }

            await uoW.SaveAsync();

            return Ok();
        }

        private bool VideoAlbumExists(int id)
        {
            return repository.Get(a => a.ID == id) != null;
        }
    }
}