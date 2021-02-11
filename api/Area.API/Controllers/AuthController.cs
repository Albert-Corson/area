using System.Net;
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
using Swashbuckle.AspNetCore.Annotations;

namespace Area.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [SwaggerTag("Authentication-related endpoints")]
    public class AuthController : ControllerBase
    {
        private readonly AuthUtilities _authUtilities;
        private readonly IConfiguration _configuration;
        private readonly UserRepository _userRepository;

        public AuthController(AuthUtilities authUtilities, UserRepository userRepository, IConfiguration configuration)
        {
            _authUtilities = authUtilities;
            _userRepository = userRepository;
            _configuration = configuration;
        }

        [HttpPost]
        [Route(RoutesConstants.Auth.SignIn)]
        [AllowAnonymous]
        [SwaggerOperation(
            Summary = "Sign-in to a user's account",
            Description = "## Get a pair of access and refresh tokens, allowing access a user's account"
        )]
        [SwaggerResponse((int) HttpStatusCode.Unauthorized, "Invalid Bearer token, identifier or password")]
        public ResponseModel<UserTokenModel> SignIn(
            [FromBody] [SwaggerRequestBody("The user's credentials", Required = true)]
            SignInModel body
        )
        {
            var encryptedPasswd = PasswordUtilities.Encrypt(_configuration[JwtConstants.SecretKeyName], body.Password!);
            if (encryptedPasswd == null)
                throw new InternalServerErrorHttpException();

            var user = _userRepository.GetUser(email: body.Identifier, passwd: encryptedPasswd)
                ?? _userRepository.GetUser(username: body.Identifier, passwd: encryptedPasswd);
            if (user == null)
                throw new UnauthorizedHttpException("Invalid identifier/password");

            return new ResponseModel<UserTokenModel> {
                Data = new UserTokenModel {
                    RefreshToken = _authUtilities.GenerateRefreshToken(user.Id),
                    AccessToken = _authUtilities.GenerateAccessToken(user.Id)
                }
            };
        }

        [HttpPost]
        [Route(RoutesConstants.Auth.RefreshAccessToken)]
        [AllowAnonymous]
        [SwaggerOperation(
            Summary = "Refresh access tokens",
            Description = "## Get a new pair of access and refresh tokens from a previous refresh token"
        )]
        public ResponseModel<UserTokenModel> RefreshAccessToken(
            [FromBody]
            [SwaggerRequestBody(
                "The refresh_token obtained from a previous call to `" + RoutesConstants.Auth.SignIn + "` or `" +
                RoutesConstants.Auth.RefreshAccessToken + "`", Required = true)]
            RefreshTokenModel body
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
        [ApiExplorerSettings(IgnoreApi = true)]
        public StatusModel RevokeUserTokens()
        {
            // TODO: (optional) revoke the credentials
            return StatusModel.Failed("Not implemented");
        }
    }
}