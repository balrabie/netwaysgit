using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using dotnetcore.Data;
using AutoMapper;
using Services;

namespace dotnetcore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private UnitOfWork uoW = null;
        private IRepository<User> repository = null;
        ServiceConfiguration emailConfig = null;
        private IMapper EntityToDtoIMapper = null;
        private IMapper DtoToEntityIMapper = null;

        private void InitializeMapping()
        {
            EntityToDtoIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<User, UserDto>())
                .CreateMapper();
            DtoToEntityIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<UserDto, User>())
                .CreateMapper();
        }

        public UserController()
        {
            uoW = new UnitOfWork();
            repository = uoW.GetRepository<User>();
            InitializeMapping();
            emailConfig = new ServiceConfiguration();
        }

        // GET: api/User
        [HttpGet]
        public IEnumerable<UserDto> Index()
        {
            List<UserDto> userDto = EntityToDtoIMapper
                .Map<List<User>, List<UserDto>>(repository.GetAll().ToList())
                .ToList();

            return userDto;
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await repository.GetAsync(a => a.ID == id);

            if (user == null)
            {
                return NotFound();
            }

            UserDto userDto = EntityToDtoIMapper.Map<User, UserDto>(user);

            return Ok(userDto);
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public async Task<ActionResult<UserDto>> PutUser([FromRoute] int id, [FromBody] UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != userDto.ID)
            {
                return BadRequest();
            }
            if (userDto.Email !=
                repository.Get(e => e.ID == id).Email)
            
            {
                return BadRequest();
            }

            if (userDto.Password.Count() < 6)
            {
                return BadRequest("Password should at least 6 characters");
            }

            User user = DtoToEntityIMapper.Map<UserDto, User>(userDto);

            repository.ModifyEntryState(user, EntityState.Modified);

            try
            {
                await uoW.SaveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            // send success email***
            emailConfig.Details(
                from: "donotreplybrd@gmail.com",
                to: userDto.Email,
                fromPassword: "brdworkshop_96##"
                );
            emailConfig.EmailManager.SendMessage("User info update",
                "User info was successfully updated");

   
            return NoContent();
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<UserDto>> PostUser([FromBody] UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User user = DtoToEntityIMapper.Map<UserDto, User>(userDto);

            repository.Add(user);
            await uoW.SaveAsync();

            return CreatedAtAction("GetUser", new { id = user.ID }, userDto);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserDto>> DeleteUser([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User user = await repository.GetAsync(a => a.ID == id);

            if (user == null)
            {
                return NotFound();
            }

            repository.Delete(user);
            await uoW.SaveAsync();

            emailConfig.Details(
                from: "donotreplybrd@gmail.com",
                to: user.Email,
                fromPassword: "brdworkshop_96##"
                );

            emailConfig.EmailManager.SendMessage("User registration",
                "User was successfully registered");

            UserDto userDto = EntityToDtoIMapper.Map<User, UserDto>(user);

            return Ok(userDto);
        }

        private bool UserExists(int id)
        {
            return repository.Get(a => a.ID == id) != null;
        }
    }
}