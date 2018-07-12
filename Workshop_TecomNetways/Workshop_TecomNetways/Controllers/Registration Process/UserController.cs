using AutoMapper;
using Services;
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

    public class UserController : ApiController
    {
        private UnitOfWork UoW = null;
        private IMapper EntityToDtoIMapper = null;
        private IMapper DtoToEntityIMapper = null;
        ServiceConfiguration emailConfig = new ServiceConfiguration();


        public UserController()
        {
            UoW = new UnitOfWork();
            InitializeMapping();
        }

        public UserController(UnitOfWork uoW)
        {
            UoW = uoW;
            InitializeMapping();
        }

        private void InitializeMapping()
        {
            EntityToDtoIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<User, UserDto>())
                .CreateMapper();
            DtoToEntityIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<UserDto, User>())
                .CreateMapper();
        }

        // GET: api/Users
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            var items = UoW.GetRepository<User>().GetAll().ToList();
            var DTO = EntityToDtoIMapper.Map<List<User>, List<UserDto>>(items).ToList();

            return Ok(DTO);
        }

        // GET: api/Users/5
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> GetUser(int id)
        {
            User item = await UoW.GetRepository<User>().GetItemAsycn(e => e.ID == id);
            if (item == null)
            {
                return NotFound();
            }

            var DTO = EntityToDtoIMapper.Map<User, UserDto>(item);
            return Ok(DTO);
        }

        // PUT: api/Users/5
        [ResponseType(typeof(void))] // NEEDS CHANGING: WE CANT MODIFY EMAIL ADDRESS OF THE USER
        public async Task<IHttpActionResult> PutUser(int id, UserDto userDto)
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
                UoW.GetRepository<User>().GetItem(e=>e.ID==id).Email) 
                // meaning we cant update the email. can be improved?
            {
                return BadRequest(); 
            }
            
            if (userDto.Password.Count() < 6)
            {
                return BadRequest("Password should at least 6 characters");
            }

            var user = DtoToEntityIMapper.Map<UserDto, User>(userDto); 
            UoW.GetRepository<User>().ModifyEntityState(user);  // update happens here

            try
            {
                await UoW.SaveAsync();
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

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Users
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> PostUser(UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (userDto.Password.Count() < 6)
            {
                return BadRequest("Password should at least 6 characters");
            }

            if (Validator.EmailIsValid(userDto.Email))
            {
                return BadRequest("Invalid Email Address");
            }

            var passwordManager = new PasswordManager(userDto.Password);

            //userDto.Salt = passwordManager.Salt;
            userDto.Password = passwordManager.HashedPassword;

            var user = DtoToEntityIMapper.Map<UserDto, User>(userDto); ////
            user.Salt = passwordManager.Salt;

            UoW.GetRepository<User>().Insert(user);
            await UoW.SaveAsync();

            emailConfig.Details(
                from: "donotreplybrd@gmail.com",
                to: userDto.Email,
                fromPassword: "brdworkshop_96##"
                );

            emailConfig.EmailManager.SendMessage("User registration",
                "User was successfully registered");

            return CreatedAtRoute("DefaultApi", new { id = user.ID }, user);
        }

        // DELETE: api/Users/5
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> DeleteUser(int id)
        {
            User user = await UoW.GetRepository<User>().GetItemAsycn(e => e.ID == id);
          
            if (user == null)
            {
                return NotFound();
            }

            UoW.GetRepository<User>().Delete(user);
            await UoW.SaveAsync();

            emailConfig.Details(
                from: "donotreplybrd@gmail.com",
                to: user.Email,
                fromPassword: "brdworkshop_96##"
                );

            emailConfig.EmailManager.SendMessage("User registration",
                "User was successfully registered");

            return Ok(user);
        }

        private bool UserExists(int id)
        {
            return UoW.GetRepository<User>().GetItem(e => e.ID == id) != null;
        }

    }
}
