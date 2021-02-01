using Area.API.Attributes;
using Area.API.Constants;
using Area.API.Exceptions.Http;
using Area.API.Models;
using Area.API.Models.Request;
using Area.API.Repositories;
using Area.API.Utilities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Area.API.Controllers
{
    public class AuthController : ControllerBase
    {
        private readonly AuthUtilities _authUtilities;
        private readonly UserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthController(AuthUtilities authUtilities, UserRepository userRepository, IConfiguration configuration)
        {
            _authUtilities = authUtilities;
            _userRepository = userRepository;
            _configuration = configuration;
        }

        [HttpPost]
        [Route(RoutesConstants.Auth.SignIn)]
        [ValidateModelState]
        public JsonResult SignIn(
            [FromBody] SignInModel body
        )
        {
            var encryptedPasswd = PasswordUtilities.Encrypt(_configuration[JwtConstants.SecretKeyName], body.Password!);
            if (encryptedPasswd == null)
                throw new InternalServerErrorHttpException();

            var user = _userRepository.GetUser(email: body.Identifier!, passwd: encryptedPasswd)
                       ?? _userRepository.GetUser(username: body.Identifier!, passwd: encryptedPasswd);
            if (user?.Id == null)
                throw new UnauthorizedHttpException("Invalid username/password");

            return new ResponseModel<UserTokenModel> {
                Data = new UserTokenModel {
                    RefreshToken = _authUtilities.GenerateRefreshToken(user.Id.Value),
                    AccessToken = _authUtilities.GenerateAccessToken(user.Id.Value)
                }
            };
        }

        [HttpPost]
        [Route(RoutesConstants.Auth.RefreshAccessToken)]
        [ValidateModelState]
        public JsonResult RefreshAccessToken(
            [FromBody] RefreshTokenModel body
        )
        {
            var userId = _authUtilities.GetUserIdFromRefreshToken(body.RefreshToken!);

            if (userId == null)
                throw new UnauthorizedHttpException();

            return new ResponseModel<UserTokenModel> {
                Data = new UserTokenModel {
                    RefreshToken = _authUtilities.GenerateRefreshToken(userId.Value),
                    AccessToken = _authUtilities.GenerateAccessToken(userId.Value)
                }
            };
        }

        [HttpDelete]
        [Route(RoutesConstants.Auth.RevokeUserTokens)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public JsonResult RevokeUserTokens()
        {
            // TODO: (optional) revoke the credentials
            return StatusModel.Failed("Not implemented");
        }
    }
}
