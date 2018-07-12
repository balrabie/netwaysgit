using AutoMapper;
using Services;
using System;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Workshop_TecomNetways.DTO;
using Workshop_TecomNetways.Models;
using Workshop_TecomNetways.Repository;


namespace Workshop_TecomNetways.Controllers
{// NEEDS CORRECTION

    public class UserTokenController : ApiController
    {
        private UnitOfWork UoW = null;
        private IMapper EntityToDtoIMapper = null;
        private IMapper DtoToEntityIMapper = null;
        ServiceConfiguration emailConfig = new ServiceConfiguration();

        public UserTokenController()
        {
            UoW = new UnitOfWork();
            InitializeMapping();
        }
        
        public UserTokenController(UnitOfWork uoW)
        {
            UoW = uoW;
            InitializeMapping();
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
            UserToken tokenUser = UoW.GetRepository<UserToken>()
                .GetItem(e => e.ID == id);
            if (tokenUser != null)
            {
                tokenUser.TokenIsUsed = true;
                await UoW.SaveAsync();
            }
        }

        private async Task<User> GetUserFromUserTokenIDAsync(int id) // id = key(UserToken)
        {
            UserToken userToken = await UoW.GetRepository<UserToken>()
                .GetItemAsycn(e => e.ID == id);

            return await UoW.GetRepository<User>()
                .GetItemAsycn(e => e.ID == userToken.UserID);
        }

        private async Task<UserToken> GetUserTokenFromUserIDAsync(int user_id) // id = key(User)
        {
            User user = await UoW.GetRepository<User>()
                .GetItemAsycn(e => e.ID == user_id);

            return await UoW.GetRepository<UserToken>()
                .GetItemAsycn(e => user.ID == e.UserID);
        }




        // POST: api/UserTokens: Generate a token WHEN (REQUEST FORGOT PASSWORD)
        [ResponseType(typeof(UserToken))] //****
        [Route("api/UserToken/{id}")]
        public async Task<IHttpActionResult> PostUserToken(int id) // using int id better than using Dto
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //var userToken = DtoToEntityIMapper.Map<UserTokenDto, UserToken>(userTokenDto); ////

            UserToken userToken = new UserToken()
            {
                UserID = id,
                Expiry = DateTime.UtcNow.Add(new TimeSpan(0, 30, 0)),
                TokenIsUsed = false,
                Token = KeyGenerator.GetUniqueKeyMixed(5),
                Email = (await GetUserFromUserTokenIDAsync(id)).Email
            };

            UoW.GetRepository<UserToken>().Insert(userToken);

            await UoW.SaveAsync();

           
            emailConfig.Details(
                from: "donotreplybrd@gmail.com",
                to: userToken.Email,
                fromPassword: "brdworkshop_96##"
                );
            emailConfig.EmailManager
                .SendMessage("Forgot Password Token!", userToken.Token); // sends the token by email

            return CreatedAtRoute("DefaultApi", new { id = userToken.ID }, userToken);
        }


        private bool UserTokenExists(int id)
        {
            return UoW.GetRepository<UserToken>().GetItem(e => e.ID == id) != null;
        }


        private bool UserExists(int id)
        {
            return UoW.GetRepository<User>().GetItem(e => e.ID == id) != null;
        }



        public async Task<bool> CheckTokenMatchAsync(int user_id, string token) // #2 *** this is used by #3
        {
           
            var userToken = await UoW.GetRepository<UserToken>()
                .GetItemAsycn(e => e.UserID == user_id 
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

        
        //update password. (assuming we requested a forget password request)
        //[HttpPut]
        [ResponseType(typeof(void))]
        [Route("api/UserToken/changepassword/{user_id}/{token}/{newPassword}")]
        public async Task<IHttpActionResult> ChangePasswordAsync(int user_id, string token, string newPassword) // #3
        {
            Action<User> ChangePassword = delegate(User user)
            {
                var passwordManager = new PasswordManager(newPassword, user.Salt);

                user.Password = passwordManager.HashedPassword;

            };

            if (await CheckTokenMatchAsync(user_id, token)) // call to #2
            {
                User user = await UoW.GetRepository<User>()
                    .GetItemAsycn(e => e.ID == user_id);

                if (user == null)
                {
                    return NotFound();
                }

                ChangePassword(user);

                try
                {
                    await UoW.SaveAsync();
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
                return StatusCode(HttpStatusCode.NoContent);
            }

            return BadRequest();
        }
        
    }
}
