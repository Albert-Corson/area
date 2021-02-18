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
using Swashbuckle.AspNetCore.Annotations;

namespace Area.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [SwaggerTag("Authentication-related endpoints")]
    public class AuthController : ControllerBase
    {
        private readonly AuthUtilities _authUtilities;
        private readonly UserRepository _userRepository;

        public AuthController(AuthUtilities authUtilities, UserRepository userRepository)
        {
            _authUtilities = authUtilities;
            _userRepository = userRepository;
        }

        [HttpPost(RouteConstants.Auth.SignIn)]
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
            var user = _userRepository.GetUser(email: body.Identifier, passwd: body.Password)
                ?? _userRepository.GetUser(username: body.Identifier, passwd: body.Password);
            if (user == null)
                throw new UnauthorizedHttpException("Invalid identifier/password");

            return new ResponseModel<UserTokenModel> {
                Data = new UserTokenModel {
                    RefreshToken = _authUtilities.GenerateRefreshToken(user.Id),
                    AccessToken = _authUtilities.GenerateAccessToken(user.Id)
                }
            };
        }

        [HttpPost(RouteConstants.Auth.RefreshAccessToken)]
        [AllowAnonymous]
        [SwaggerOperation(
            Summary = "Refresh access tokens",
            Description = "## Get a new pair of access and refresh tokens from a previous refresh token"
        )]
        public ResponseModel<UserTokenModel> RefreshAccessToken(
            [FromBody]
            [SwaggerRequestBody(
                "The refresh_token obtained from a previous call to `" + RouteConstants.Auth.SignIn + "` or `" +
                RouteConstants.Auth.RefreshAccessToken + "`", Required = true)]
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

        [HttpDelete(RouteConstants.Auth.RevokeUserTokens)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public StatusModel RevokeUserTokens()
        {
            // TODO: (optional) revoke the credentials
            return StatusModel.Failed("Not implemented");
        }
    }
}