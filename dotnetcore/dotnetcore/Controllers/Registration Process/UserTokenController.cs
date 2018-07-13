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
    [Route("api/UserToken")]
    [ApiController]
    public class UserTokenController : ControllerBase
    {
        private UnitOfWork uoW = null;
        private IRepository<UserToken> userTokenRepository = null;
        private IRepository<User> userRepository = null;
        private IMapper EntityToDtoIMapper = null;
        private IMapper DtoToEntityIMapper = null;

        ServiceConfiguration emailConfig = null;
        
        public UserTokenController()
        {
            uoW = new UnitOfWork();
            userTokenRepository = uoW.GetRepository<UserToken>();
            userRepository = uoW.GetRepository<User>();
            InitializeMapping();
            emailConfig = new ServiceConfiguration();
        }

        private void InitializeMapping()
        {
            EntityToDtoIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<UserToken, UserTokenDto>())
                .CreateMapper();
            DtoToEntityIMapper = new MapperConfiguration
                (cfg => cfg.CreateMap<UserTokenDto, UserToken>())
                .CreateMapper();
        }
        
        private async Task DeactivateTokenAsync(int id)
        {
            UserToken tokenUser = userTokenRepository
                .Get(e => e.ID == id);
            if (tokenUser != null)
            {
                tokenUser.TokenIsUsed = true;
                await uoW.SaveAsync();
            }
        }
        
        private async Task<User> GetUserFromUserTokenIDAsync(int id) // id = key(UserToken)
        {
            UserToken userToken = await userTokenRepository
                .GetAsync(e => e.ID == id);

            return await userRepository
                .GetAsync(e => e.ID == userToken.UserID);
        }

        private async Task<UserToken> GetUserTokenFromUserIDAsync(int user_id) // id = key(User)
        {
            User user = await userRepository
                .GetAsync(e => e.ID == user_id);

            return await userTokenRepository
                .GetAsync(e => user.ID == e.UserID);
        }

        
        private async Task<bool> CheckTokenMatchAsync(int user_id, string token) // #2 *** (helper)
        {

            var userToken = await userTokenRepository
                .GetAsync(e => e.UserID == user_id
                && e.Token == token
                && !e.TokenIsUsed
                && e.Expiry.CompareTo(DateTime.UtcNow) > 1);

            if (userToken != null)
            {
                await DeactivateTokenAsync(user_id);
                return true;
            }

            return false;
        }
        
        
        // GET: api/UserToken
        [HttpGet]
        public IEnumerable<UserTokenDto> Index()
        {
            List<UserTokenDto> userTokenDto = EntityToDtoIMapper
                .Map<List<UserToken>, List<UserTokenDto>>(userTokenRepository.GetAll().ToList())
                .ToList();

            return userTokenDto;
        }

        
        // POST: api/UserToken/5
        [HttpPost("{id}")]
        public async Task<ActionResult<UserToken>> PostUserToken([FromRoute] int id) // #1
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            UserToken userToken = new UserToken()
            {
                UserID = id,
                Expiry = DateTime.UtcNow.Add(new TimeSpan(0, 30, 0)),
                TokenIsUsed = false,
                Token = KeyGenerator.GetUniqueKeyMixed(5),
                Email = (await GetUserFromUserTokenIDAsync(id)).Email
            };

            userTokenRepository.Add(userToken);

            await uoW.SaveAsync();


            emailConfig.Details(
                from: "donotreplybrd@gmail.com",
                to: userToken.Email,
                fromPassword: "brdworkshop_96##"
                );
            emailConfig.EmailManager
                .SendMessage("Forgot Password Token!", userToken.Token); // sends the token by email

            return CreatedAtRoute("DefaultApi", new { id = userToken.ID }, userToken);
        }


        // PUT: api/UserTokens/changepassword/{user_id}
        [HttpPut("Changepassword/{user_id}")]
        public async Task<ActionResult<UserToken>> 
            ChangePasswordAsync([FromRoute] int user_id, [FromBody] ChangePasswordInfo info) // #3
        {
            Action<User> ChangePassword = delegate (User user)
            {
                var passwordManager = new PasswordManager(info.NewPassword, user.Salt);

                user.Password = passwordManager.HashedPassword;

            };

            if (await CheckTokenMatchAsync(user_id, info.Token)) // call to #2
            {
                User user = await userRepository
                    .GetAsync(e => e.ID == user_id);

                if (user == null)
                {
                    return NotFound();
                }

                ChangePassword(user);

                try
                {
                    await uoW.SaveAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user_id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                emailConfig.Details(
                    from: "donotreplybrd@gmail.com",
                    to: user.Email,
                    fromPassword: "brdworkshop_96##"
                    );
                emailConfig.EmailManager.SendMessage("Account Update",
                    "You have successfully changed your password");
                return NoContent();
            }

            return BadRequest();
        }
        
    
        private bool UserTokenExists(int id)
        {
            return userTokenRepository.Get(e => e.ID == id) != null;
        }


        private bool UserExists(int id)
        {
            return userRepository.Get(e => e.ID == id) != null;
        }

        
    }
}