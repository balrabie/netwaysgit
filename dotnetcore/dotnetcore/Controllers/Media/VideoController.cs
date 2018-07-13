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
    public class VideoController : ControllerBase
    {
        private UnitOfWork uoW = null;
        private IRepository<Video> repository = null;

        private IMapper EntityToDtoIMapper = null;
        private IMapper DtoToEntityIMapper = null;

        private void InitializeMapping()
        {
            EntityToDtoIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<Video, VideoDto>())
                .CreateMapper();
            DtoToEntityIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<VideoDto, Video>())
                .CreateMapper();
        }

        public VideoController()
        {
            uoW = new UnitOfWork();
            repository = uoW.GetRepository<Video>();
            InitializeMapping();
        }

        // GET: api/Video
        [HttpGet]
        public IEnumerable<VideoDto> Index()
        {
            List<VideoDto> videoDto = EntityToDtoIMapper
                .Map<List<Video>, List<VideoDto>>(repository.GetAll().ToList())
                .ToList();

            return videoDto;
        }

        // GET: api/Video/5
        [HttpGet("{id}")]
        public async Task<ActionResult<VideoDto>> GetVideo([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var video = await repository.GetAsync(a => a.ID == id);

            if (video == null)
            {
                return NotFound();
            }

            VideoDto videoDto = EntityToDtoIMapper.Map<Video, VideoDto>(video);

            return Ok(videoDto);
        }

        // PUT: api/Video/5
        [HttpPut("{id}")]
        public async Task<ActionResult<VideoDto>> PutVideo([FromRoute] int id, [FromBody] VideoDto videoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != videoDto.ID)
            {
                return BadRequest();
            }

            Video video = DtoToEntityIMapper.Map<VideoDto, Video>(videoDto);

            repository.ModifyEntryState(video, EntityState.Modified);

            try
            {
                await uoW.SaveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VideoExists(id))
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

        // POST: api/Videos
        [HttpPost]
        public async Task<ActionResult<VideoDto>> PostVideo([FromBody] VideoDto videoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Video video = DtoToEntityIMapper.Map<VideoDto, Video>(videoDto);      

            repository.Add(video);

            await uoW.SaveAsync();

            return CreatedAtAction("GetVideo", new { id = video.ID }, videoDto);
        }

        // DELETE: api/Videos/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<VideoDto>> DeleteVideo([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Video video = await repository.GetAsync(a => a.ID == id);

            if (video == null)
            {
                return NotFound();
            }

            repository.Delete(video);
            await uoW.SaveAsync();

            VideoDto videoDto = EntityToDtoIMapper.Map<Video, VideoDto>(video);

            return Ok(videoDto);
        }

        [HttpPut("{id}/LinkedTo")]
        public async Task<ActionResult<VideoDto>> AssociateWithVideos([FromRoute] int id, [FromBody] int[] albumIDs)
        {
            Video video = await repository.GetAsync(e => e.ID == id);

            if (video == null)
            {
                return BadRequest();
            }

            foreach (int albumID in albumIDs)
            {
                VideoAlbum videoAlbum = await uoW.GetRepository<VideoAlbum>().GetAsync(e => e.ID == id);
                if (videoAlbum == null)
                {
                    continue;
                }

                var relation = new VideoAlbumVideo()
                {
                    VideoAlbumID = albumID,
                    VideoID = id,
                    Video = video,
                    VideoAlbum = videoAlbum
                };

                if (videoAlbum.Videos == null)
                {
                    video.VideoAlbums = new List<VideoAlbumVideo>();
                    
                }
                repository.ModifyEntryState(video, EntityState.Modified);
                video.VideoAlbums.Add(relation);
            }

            await uoW.SaveAsync();

            return Ok();
        }

        private bool VideoExists(int id)
        {
            return repository.Get(a => a.ID == id) != null;
        }
    }
}