using System.ComponentModel.DataAnnotations;
using System.Linq;
using Area.API.Attributes;
using Area.API.Common;
using Area.API.Constants;
using Area.API.Database;
using Area.API.Exceptions.Http;
using Area.API.Models;
using Area.API.Models.Request;
using Area.API.Models.Table;
using Area.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Area.API.Controllers
{
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly DbContext _database;

        public UsersController(DbContext database, IConfiguration configuration)
        {
            _database = database;
            _configuration = configuration;
        }

        [HttpPost]
        [Route(RoutesConstants.Users.SignUp)]
        [ValidateModelState]
        public JsonResult SignUp(
            [FromBody] RegisterModel body
        )
        {
            var existingUser = _database.Users.FirstOrDefault(model => model.Email == body.Email || model.Username == body.Username);

            if (existingUser != null)
                throw new ConflictHttpException("Username or email already in use");

            var encryptedPasswd = Encryptor.Encrypt(_configuration[JwtConstants.SecretKeyName], body.Password!);

            if (encryptedPasswd == null)
                throw new InternalServerErrorHttpException();

            _database.Users.Add(new UserModel {
                Username = body.Username,
                Password = encryptedPasswd,
                Email = body.Email
            });
            _database.SaveChanges();
            return StatusModel.Success();
        }

        [HttpGet]
        [Route(RoutesConstants.Users.GetUser)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ValidateModelState]
        public JsonResult GetUser(
            [FromRoute] [Required] [Range(1, 2147483647)] int? userId
        )
        {
            var user = _database.Users.FirstOrDefault(model => model.Id == userId);

            if (user == null)
                throw new NotFoundHttpException();

            return new ResponseModel<UserModel> {
                Data = new UserModel {
                    Id = user.Id,
                    Username = user.Username
                }
            };
        }

        [HttpGet]
        [Route(RoutesConstants.Users.GetMyUser)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public JsonResult GetMyUser()
        {
            var userId = AuthService.GetUserIdFromPrincipal(User);
            var user = _database.Users.First(model => model.Id == userId);

            return new ResponseModel<UserModel> {
                Data = user
            };
        }

        [HttpDelete]
        [Route(RoutesConstants.Users.DeleteUser)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ValidateModelState]
        public JsonResult DeleteUser(
            [FromRoute] [Required] [Range(1, 2147483647)] int? userId
        )
        {
            var currentUserId = AuthService.GetUserIdFromPrincipal(User);

            if (currentUserId != userId)
                throw new UnauthorizedHttpException("You can only delete your own account");

            var user = _database.Users.First(model => model.Id == userId);

            _database.Users.Remove(user);
            _database.SaveChanges();

            return StatusModel.Success();
        }
    }
}
