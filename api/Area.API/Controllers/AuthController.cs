using System;
using System.Collections.Specialized;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Area.API.Attributes;
using Area.API.Constants;
using Area.API.Exceptions.Http;
using Area.API.Extensions;
using Area.API.Models;
using Area.API.Models.Request;
using Area.API.Models.Table;
using Area.API.Repositories;
using Area.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OAuth2.Client.Impl;
using OAuth2.Configuration;
using OAuth2.Infrastructure;
using OAuth2.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace Area.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [SwaggerTag("Authentication-related endpoints")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly UserRepository _userRepository;
        private readonly FacebookClient _facebook;

        public AuthController(AuthService authService, UserRepository userRepository, IConfiguration configuration)
        {
            _authService = authService;
            _userRepository = userRepository;
            _facebook = new FacebookClient(new RequestFactory(), new ClientConfiguration {
                ClientId = configuration[AuthConstants.Facebook.ClientId],
                ClientSecret = configuration[AuthConstants.Facebook.ClientSecret],
                RedirectUri = configuration[AuthConstants.Facebook.RedirectUri],
                Scope = "email"
            });
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
            var principal = await _authService.ValidateRefreshToken(body.RefreshToken, HttpContext.Connection.RemoteIpAddress.MapToIPv4());

            if (principal == null || !principal.TryGetUserId(out var userId))
                throw new UnauthorizedHttpException();

            return new ResponseModel<UserTokenModel> {
                Data = new UserTokenModel {
                    RefreshToken = await _authService.GenerateRefreshToken(userId, HttpContext.Connection.RemoteIpAddress),
                    AccessToken = await _authService.GenerateAccessToken(userId, HttpContext.Connection.RemoteIpAddress)
                }
            };
        }

        [HttpPost(RouteConstants.Auth.ExchangeCode)]
        [AllowAnonymous]
        [SwaggerOperation(
            Summary = "Exchange authentication code",
            Description = "Exchange an authentication code obtained from one of the external sign-in services endpoints, and receive access and refresh tokens"
        )]
        public async Task<ResponseModel<UserTokenModel>> ExchangeCode(
            [FromBody]
            [SwaggerRequestBody("Authentication code obtained from one of the external sign-in services endpoints, along with the id and secret of the client")]
            ExchangeCodeModel body
        )
        {
            if (!_authService.TryGetPrincipalFromToken(body.Code, out var principal)
                || !principal.TryGetUserId(out var userId))
                throw new UnauthorizedHttpException();

            var user = _userRepository.GetUser(userId, asNoTracking: false);
            if (user == null)
                throw new UnauthorizedHttpException();

            var claim = _userRepository.GetUserClaim(user.Id, user.Type.ToString());
            if (claim == null || claim.ClaimValue != body.Code)
                throw new UnauthorizedHttpException();

            var identityResult = await _userRepository.RemoveUserClaim(user, claim.ToClaim());
            if (!identityResult.Succeeded)
                throw new InternalServerErrorHttpException();

            return new ResponseModel<UserTokenModel> {
                Data = new UserTokenModel {
                    RefreshToken = await _authService.GenerateRefreshToken(user.Id, HttpContext.Connection.RemoteIpAddress),
                    AccessToken = await _authService.GenerateAccessToken(user.Id, HttpContext.Connection.RemoteIpAddress)
                }
            };
        }

        [HttpPost(RouteConstants.Auth.SignInWithFacebook)]
        [AllowAnonymous]
        [ValidateModelState(false)]
        [SwaggerOperation(
            Summary = "Sign-in/register with Facebook",
            Description = "Sign-in/register a user, using their Facebook account"
        )]
        public async Task<IActionResult> SignInWithFacebook(
            CancellationToken cancellationToken,
            [FromBody]
            [SwaggerRequestBody("Mandatory information to be able to redirect the url back to the client once the operation is done")]
            ExternalAuthModel body
            )
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var state = HttpUtility.UrlEncode(JsonConvert.SerializeObject(body));

            return new RedirectResult(await _facebook.GetLoginLinkUriAsync(state, cancellationToken));
        }

        [HttpGet(RouteConstants.Auth.SignInWithFacebookCallback)]
        [AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<RedirectResult> SignInWithFacebookCallback(
            CancellationToken cancellationToken,
            [FromQuery] string code,
            [FromQuery] string state
        )
        {
            var authRequestBody = JsonConvert.DeserializeObject<ExternalAuthModel>(HttpUtility.UrlDecode(state));

            var urlBuild = new UriBuilder(authRequestBody.RedirectUrl);
            var query = HttpUtility.ParseQueryString(urlBuild.Query);

            if (authRequestBody.State != null)
                query["state"] = authRequestBody.State;

            try {
                UserInfo userInfo = await _facebook.GetUserInfoAsync(new NameValueCollection {{nameof(code), code}}, cancellationToken);

                var authResult = await _authService.AuthenticateExternalUserAsync(userInfo, UserModel.UserType.Facebook);

                if (authResult.Successful) {
                    query["code"] = authResult.Code;
                    query["successful"] = "true";
                } else {
                    query["error"] = authResult.Error;
                    query["successful"] = "false";
                }
            } catch {
               query["successful"] = "false";
               query["error"] = "Authentication canceled";
            }

            urlBuild.Query = query.ToString();
            return new RedirectResult(urlBuild.ToString());
        }
    }
}