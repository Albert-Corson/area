using System.ComponentModel.DataAnnotations;
using Area.API.Attributes;
using Area.API.Constants;
using Area.API.Exceptions.Http;
using Area.API.Models;
using Area.API.Models.Request;
using Area.API.Models.Table;
using Area.API.Repositories;
using Area.API.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Area.API.Controllers
{
    public class UsersController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public UsersController(UserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        [HttpPost]
        [Route(RoutesConstants.Users.Register)]
        [ValidateModelState]
        public JsonResult Register(
            [FromBody] RegisterModel body
        )
        {
            if (!AuthUtilities.IsValidEmail(body.Email!))
                throw new BadRequestHttpException("Please provide a valid email address");

            if (PasswordUtilities.IsWeakPassword(body.Password!))
                throw new BadRequestHttpException(
                    "Password too weak. At least 8 characters, with and without capitals, with numerical and special characters required.");

            if (!UsernameUtilities.IsUsernameValid(body.Username!))
                throw new BadRequestHttpException("Invalid username");

            if (_userRepository.UserExists(email: body.Email, username: body.Username))
                throw new ConflictHttpException("Username or email already in use");

            var encryptedPasswd = PasswordUtilities.Encrypt(_configuration[JwtConstants.SecretKeyName], body.Password!);
            if (encryptedPasswd == null)
                throw new InternalServerErrorHttpException();

            _userRepository.AddUser(new UserModel {
                Username = body.Username,
                Password = encryptedPasswd,
                Email = body.Email
            });

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
            var user = _userRepository.GetUser(userId);

            if (user == null)
                throw new NotFoundHttpException("This user does not exist");

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
            var userId = AuthUtilities.GetUserIdFromPrincipal(User);

            var user = _userRepository.GetUser(userId);

            return new ResponseModel<UserModel> {
                Data = user!
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
            var currentUserId = AuthUtilities.GetUserIdFromPrincipal(User);

            if (currentUserId != userId)
                throw new UnauthorizedHttpException("You can only delete your own account");

            if (!_userRepository.RemoveUser(userId!.Value))
                throw new InternalServerErrorHttpException(); // this should never happen, but we still have to handle the error

            return StatusModel.Success();
        }
    }
}