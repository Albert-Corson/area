using System.Net;
using System.Threading.Tasks;
using Area.API.Constants;
using Area.API.Exceptions.Http;
using Area.API.Models;
using Area.API.Models.Request;
using Area.API.Repositories;
using Area.API.Services;
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
        private readonly AuthService _authService;
        private readonly UserRepository _userRepository;

        public AuthController(AuthService authService, UserRepository userRepository)
        {
            _authService = authService;
            _userRepository = userRepository;
        }

        [HttpPost(RouteConstants.Auth.SignIn)]
        [AllowAnonymous]
        [SwaggerOperation(
            Summary = "Sign-in to a user's account",
            Description = "## Get a pair of access and refresh tokens, allowing access a user's account"
        )]
        [SwaggerResponse((int) HttpStatusCode.Unauthorized, "Invalid Bearer token, identifier or password")]
        public async Task<ResponseModel<UserTokenModel>> SignIn(
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
                    RefreshToken = await _authService.GenerateRefreshToken(user.Id, HttpContext.Connection.RemoteIpAddress),
                    AccessToken = await _authService.GenerateAccessToken(user.Id, HttpContext.Connection.RemoteIpAddress)
                }
            };
        }

        [HttpPost(RouteConstants.Auth.RefreshAccessToken)]
        [AllowAnonymous]
        [SwaggerOperation(
            Summary = "Refresh access tokens",
            Description = "## Get a new pair of access and refresh tokens from a previous refresh token"
        )]
        public async Task<ResponseModel<UserTokenModel>> RefreshAccessToken(
            [FromBody]
            [SwaggerRequestBody(
                "The refresh_token obtained from a previous call to `" + RouteConstants.Auth.SignIn + "` or `" +
                RouteConstants.Auth.RefreshAccessToken + "`", Required = true)]
            RefreshTokenModel body
        )
        {
            var userId = _authService.GetUserIdFromRefreshToken(body.RefreshToken!);

            if (userId == null)
                throw new UnauthorizedHttpException();

            return new ResponseModel<UserTokenModel> {
                Data = new UserTokenModel {
                    RefreshToken = await _authService.GenerateRefreshToken(userId.Value, HttpContext.Connection.RemoteIpAddress),
                    AccessToken = await _authService.GenerateAccessToken(userId.Value, HttpContext.Connection.RemoteIpAddress)
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