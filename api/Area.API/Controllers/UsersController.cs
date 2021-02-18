using System.Linq;
using System.Net;
using System.Threading.Tasks;
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
            Description =
                "## Create a new user account"
        )]
        [SwaggerResponse((int) HttpStatusCode.BadRequest,
            "Malformed body, incorrect username/email format, password too weak")]
        [SwaggerResponse((int) HttpStatusCode.Conflict, "Username or email already in use")]
        public async Task<StatusModel> Register(
            [FromBody]
            [SwaggerSchema(
                "The user's information. The password must be at least 8 characters long, with and without capitals, with numerical and special characters. The username must start with a letter, numeric characters and some special characters (._-) are accepted")]
            RegisterModel body
        )
        {
            if (_userRepository.UserExists(email: body.Email, username: body.Username))
                throw new ConflictHttpException("Username or email already in use");

            var user = new UserModel {
                UserName = body.Username,
                Email = body.Email
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
            var userId = AuthUtilities.GetUserIdFromPrincipal(User);
            var user = _userRepository.GetUser(userId);

            if (user == null)
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
            var userId = AuthUtilities.GetUserIdFromPrincipal(User);

            if (userId == null || !_userRepository.RemoveUser(userId!.Value))
                throw new InternalServerErrorHttpException(); // this should never happen, but we still have to handle the error

            return StatusModel.Success();
        }
    }
}