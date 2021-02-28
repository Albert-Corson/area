using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Area.API.Constants;
using Area.API.Exceptions.Http;
using Area.API.Extensions;
using Area.API.Models;
using Area.API.Models.Request;
using Area.API.Models.Table;
using Area.API.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Area.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [SwaggerTag("User-related endpoints")]
    public class UsersController : ControllerBase
    {
        private readonly UserRepository _userRepository;

        public UsersController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost(RouteConstants.Users.Register)]
        [AllowAnonymous]
        [SwaggerOperation(
            Summary = "Register a user",
            Description = "## Create a new user account"
        )]
        [SwaggerResponse((int) HttpStatusCode.BadRequest, "Malformed body, incorrect username/email format, password too weak")]
        [SwaggerResponse((int) HttpStatusCode.Conflict, "Username or email already in use")]
        public async Task<StatusModel> Register(
            [FromBody]
            [SwaggerRequestBody(
                "The user's information. The password must be at least 8 characters long, with and without capitals, with numerical and special characters. The username must start with a letter, numeric characters and some special characters (._-) are accepted")]
            RegisterModel body
        )
        {
            if (body.Username.Length < 4)
                throw new BadRequestHttpException("Username must be at least 4 characters long");

            if (_userRepository.UserExists(email: body.Email, username: body.Username))
                throw new ConflictHttpException("Username or email already in use");

            var user = new UserModel {
                UserName = body.Username,
                Email = body.Email,
                Type = UserModel.UserType.Area
            };

            var result = await _userRepository.AddUser(user, body.Password);

            if (!result.Succeeded)
                throw new BadRequestHttpException(result.Errors.First().Description);
            return StatusModel.Success();
        }

        [HttpGet(RouteConstants.Users.GetMyUser)]
        [SwaggerOperation(
            Summary = "Get the current user's information",
            Description =
                "## Get information about the current user associated to the bearer token used for the request"
        )]
        public ResponseModel<UserInformationModel> GetMyUser()
        {
            if (!User.TryGetUser(_userRepository, out var user))
                throw new InternalServerErrorHttpException(); // should never happen

            return new ResponseModel<UserInformationModel> {
                Data = new UserInformationModel(user)
            };
        }

        [HttpDelete(RouteConstants.Users.DeleteMyUser)]
        [SwaggerOperation(
            Summary = "Delete a user",
            Description = "## Delete a user's account"
        )]
        [SwaggerResponse((int) HttpStatusCode.Unauthorized, "Not allowed to delete the desired user")]
        public StatusModel DeleteUser()
        {
            UserModel? user = null;

            if (User.TryGetUserId(out var userId))
                user = _userRepository.GetUser(userId, asNoTracking: false);

            if (user == null)
                throw new InternalServerErrorHttpException(); // this should never happen, but we still have to handle the error

            _userRepository.RemoveUser(user);
            return StatusModel.Success();
        }

        [HttpGet(RouteConstants.Users.GetMyDevices)]
        [SwaggerOperation(
            Summary = "Get a user's known devices",
            Description = "## Get a list of devices associated to the user's account"
        )]
        public ResponseModel<UserDevicesModel> GetMyDevices()
        {
            User.TryGetUser(_userRepository, out var user);
            User.TryGetDeviceId(out var deviceId);

            return new ResponseModel<UserDevicesModel> {
                Data = new UserDevicesModel {
                    CurrentDevice = deviceId,
                    Devices = user!.Devices.ToList()
                }
            };
        }

        [HttpDelete(RouteConstants.Users.DeleteMyDevice)]
        [SwaggerOperation(
            Summary = "Forget a user's device",
            Description = "## Forget a user's device and revoke the access and refresh tokens created from this device"
        )]
        [SwaggerResponse((int) HttpStatusCode.NotFound, "Invalid deviceId")]
        public StatusModel DeleteMyDevice(
            [FromRoute] [Required] [Range(1, uint.MaxValue)] [SwaggerParameter("Device's ID")]
            uint? deviceId
        )
        {
            User.TryGetUserId(out var userId);

            if (!_userRepository.RemoveDevice(userId, deviceId!.Value))
                throw new NotFoundHttpException("This device doesn't exist");

            return StatusModel.Success();
        }
    }
}