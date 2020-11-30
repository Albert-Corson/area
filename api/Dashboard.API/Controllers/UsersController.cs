using System.ComponentModel.DataAnnotations;
using System.Linq;
using Dashboard.API.Attributes;
using Dashboard.API.Common;
using Dashboard.API.Constants;
using Dashboard.API.Exceptions.Http;
using Dashboard.API.Models.Request;
using Dashboard.API.Models.Response;
using Dashboard.API.Models.Table;
using Dashboard.API.Repositories;
using Dashboard.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Dashboard.API.Controllers
{
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly DatabaseRepository _database;

        public UsersController(DatabaseRepository database, IConfiguration configuration)
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

            // TODO: (optional) Check for weak or invalid email address
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
                Data = {
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

            if (userId == null)
                throw new UnauthorizedHttpException();

            var user = _database.Users.FirstOrDefault(model => model.Id == userId);
            if (user == null)
                throw new NotFoundHttpException("This access token may belong to a deleted user");

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

            if (currentUserId == null || currentUserId != userId)
                throw new UnauthorizedHttpException("You can only delete your own account");

            var user = _database.Users.FirstOrDefault(model => model.Id == userId);

            if (user == null)
                throw new NotFoundHttpException();

            _database.Users.Remove(user);
            _database.SaveChanges();

            return StatusModel.Success();
        }
    }
}
