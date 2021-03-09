using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
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
using Microsoft.Graph;
using Microsoft.Graph.Auth;
using Microsoft.Identity.Client;
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
        private readonly GoogleClient _google;
        private readonly IConfidentialClientApplication _microsoft;

        public AuthController(AuthService authService, UserRepository userRepository, IConfiguration configuration)
        {
            _authService = authService;
            _userRepository = userRepository;
            var factory = new RequestFactory();
            _facebook = new FacebookClient(factory, new ClientConfiguration {
                ClientId = configuration[AuthConstants.Facebook.ClientId],
                ClientSecret = configuration[AuthConstants.Facebook.ClientSecret],
                RedirectUri = configuration[AuthConstants.Facebook.RedirectUri],
                Scope = "email"
            });
            _google = new GoogleClient(factory, new ClientConfiguration {
                ClientId = configuration[AuthConstants.Google.ClientId],
                ClientSecret = configuration[AuthConstants.Google.ClientSecret],
                RedirectUri = configuration[AuthConstants.Google.RedirectUri],
                Scope = "profile email"
            });
            _microsoft = ConfidentialClientApplicationBuilder
                .Create(configuration[AuthConstants.Microsoft.ClientId])
                .WithClientSecret(configuration[AuthConstants.Microsoft.ClientSecret])
                .WithRedirectUri(configuration[AuthConstants.Microsoft.AuthRedirectUri])
                .Build();
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
        [SwaggerOperation(
            Summary = "Sign-in/register with Facebook",
            Description = "Redirect the user to this endpoint to let them sign-in/register with Facebook. The user will be redirected back to the client with an authentication code once done."
        )]
        [SwaggerResponse((int) HttpStatusCode.Found, "Redirection to Facebook's sign-in page")]
        public async Task<RedirectResult> SignInWithFacebook(
            CancellationToken cancellationToken,
            [FromBody]
            [SwaggerRequestBody("Mandatory information to be able to redirect the user back to the client once the operation is done")]
            ExternalAuthModel body
            )
        {
            var state = JsonConvert.SerializeObject(body);

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
            var authRequestBody = JsonConvert.DeserializeObject<ExternalAuthModel>(state);

            var redirectUrl = new UriBuilder(authRequestBody.RedirectUrl);
            var query = HttpUtility.ParseQueryString(redirectUrl.Query);

            if (authRequestBody.State != null)
                query["state"] = authRequestBody.State;

            try {
                UserInfo userInfo = await _facebook.GetUserInfoAsync(new NameValueCollection {{nameof(code), code}}, cancellationToken);

                var authResult = await _authService.AuthenticateExternalUserAsync(userInfo.Email, UserModel.UserType.Facebook);

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

            redirectUrl.Query = query.ToString();
            return new RedirectResult(redirectUrl.ToString());
        }

        [HttpPost(RouteConstants.Auth.SignInWithGoogle)]
        [AllowAnonymous]
        [SwaggerOperation(
            Summary = "Sign-in/register with Google",
            Description = "Redirect the user to this endpoint to let them sign-in/register with Google. The user will be redirected back to the client with an authentication code once done."
        )]
        [SwaggerResponse((int) HttpStatusCode.Found, "Redirection to Google's sign-in page")]
        public async Task<RedirectResult> SignInWithGoogle(
            CancellationToken cancellationToken,
            [FromBody]
            [SwaggerRequestBody("Mandatory information to be able to redirect the user back to the client once the operation is done")]
            ExternalAuthModel body
            )
        {
            var state = JsonConvert.SerializeObject(body);

            return new RedirectResult(await _google.GetLoginLinkUriAsync(state, cancellationToken));
        }

        [HttpGet(RouteConstants.Auth.SignInWithGoogleCallback)]
        [AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<RedirectResult> SignInWithGoogleCallback(
            CancellationToken cancellationToken,
            [FromQuery] string code,
            [FromQuery] string state
        )
        {
            var authRequestBody = JsonConvert.DeserializeObject<ExternalAuthModel>(state);

            var redirectUrl = new UriBuilder(authRequestBody.RedirectUrl);
            var query = HttpUtility.ParseQueryString(redirectUrl.Query);

            if (authRequestBody.State != null)
                query["state"] = authRequestBody.State;

            try {
                UserInfo userInfo = await _google.GetUserInfoAsync(new NameValueCollection {{nameof(code), code}}, cancellationToken);

                var authResult = await _authService.AuthenticateExternalUserAsync(userInfo.Email, UserModel.UserType.Google);

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

            redirectUrl.Query = query.ToString();
            return new RedirectResult(redirectUrl.ToString());
        }

        [HttpPost(RouteConstants.Auth.SignInWithMicrosoft)]
        [AllowAnonymous]
        [SwaggerOperation(
            Summary = "Sign-in/register with Microsoft",
            Description = "Redirect the user to this endpoint to let them sign-in/register with Microsoft. The user will be redirected back to the client with an authentication code once done."
        )]
        [SwaggerResponse((int) HttpStatusCode.Found, "Redirection to Microsoft's sign-in page")]
        public async Task<RedirectResult> SignInWithMicrosoft(
            CancellationToken cancellationToken,
            [FromBody]
            [SwaggerRequestBody("Mandatory information to be able to redirect the user back to the client once the operation is done")]
            ExternalAuthModel body
            )
        {
            var uri =  await _microsoft.GetAuthorizationRequestUrl(new[] {"user.read"})
                .WithExtraQueryParameters(new Dictionary<string, string> {
                    {"state", JsonConvert.SerializeObject(body)}
                })
                .ExecuteAsync(cancellationToken);

            return new RedirectResult(uri.ToString());
        }

        [HttpGet(RouteConstants.Auth.SignInWithMicrosoftCallback)]
        [AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<RedirectResult> SignInWithMicrosoftCallback(
            CancellationToken cancellationToken,
            [FromQuery] string code,
            [FromQuery] string state
        )
        {
            var authRequestBody = JsonConvert.DeserializeObject<ExternalAuthModel>(state);

            var redirectUrl = new UriBuilder(authRequestBody.RedirectUrl);
            var query = HttpUtility.ParseQueryString(redirectUrl.Query);

            if (authRequestBody.State != null)
                query["state"] = authRequestBody.State;

            try {
                var provider = new AuthorizationCodeProvider(_microsoft, new[] {"user.read"});
                await provider.GetAccessTokenByAuthorizationCode(code);

                var userInfo = await new GraphServiceClient(provider).Me.Request().GetAsync(cancellationToken);

                var authResult = await _authService.AuthenticateExternalUserAsync(userInfo.Mail, UserModel.UserType.Microsoft);

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

            redirectUrl.Query = query.ToString();
            return new RedirectResult(redirectUrl.ToString());
        }
    }
}