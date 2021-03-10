using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
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
                throw new BadRequestHttpException();

            var user = _userRepository.GetUser(userId, asNoTracking: false);
            if (user == null)
                throw new BadRequestHttpException();

            var claim = _userRepository.GetUserClaim(user.Id, user.Type.ToString());
            if (claim == null || claim.ClaimValue != body.Code)
                throw new BadRequestHttpException();

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

        [HttpGet(RouteConstants.Auth.SignInWithFacebook)]
        [AllowAnonymous]
        [SwaggerOperation(
            Summary = "Sign-in/register with Facebook",
            Description = "Get a URL to redirect the user to, to let them sign-in/register with Facebook. The user will be redirected back to the client with an authentication code once done."
        )]
        public async Task<ResponseModel<AuthenticationRedirectModel>> SignInWithFacebook(
            [Required, FromQuery(Name = "redirect_url")]
            [SwaggerParameter("The URL to redirect the user to once the operation is completed")]
            string? redirectUrl,

            [FromQuery(Name = "state")]
            [SwaggerParameter("A freely-defined value that will sent back to the client")]
            string? clientState
            )
        {
            return await SignInWithExternalService(redirectUrl!, clientState,
                async state => new Uri(await _facebook.GetLoginLinkUriAsync(state)));
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
            return await SignInWithExternalServiceCallback(state, UserModel.UserType.Facebook, async () => {
                UserInfo userInfo = await _facebook.GetUserInfoAsync(new NameValueCollection {{nameof(code), code}}, cancellationToken);

                return userInfo.Email;
            });
        }

        [HttpGet(RouteConstants.Auth.SignInWithGoogle)]
        [AllowAnonymous]
        [SwaggerOperation(
            Summary = "Sign-in/register with Google",
            Description = "Get a URL to redirect the user to, to let them sign-in/register with Google. The user will be redirected back to the client with an authentication code once done."
        )]
        public async Task<ResponseModel<AuthenticationRedirectModel>> SignInWithGoogle(
            [Required, FromQuery(Name = "redirect_url")]
            [SwaggerParameter("The URL to redirect the user to once the operation is completed")]
            string? redirectUrl,

            [FromQuery(Name = "state")]
            [SwaggerParameter("A freely-defined value that will sent back to the client")]
            string? clientState
            )
        {
            return await SignInWithExternalService(redirectUrl!, clientState,
                async state => new Uri(await _google.GetLoginLinkUriAsync(state)));
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
            return await SignInWithExternalServiceCallback(state, UserModel.UserType.Google, async () => {
                UserInfo userInfo = await _google.GetUserInfoAsync(new NameValueCollection {{nameof(code), code}}, cancellationToken);

                return userInfo.Email;
            });
        }

        [HttpGet(RouteConstants.Auth.SignInWithMicrosoft)]
        [AllowAnonymous]
        [SwaggerOperation(
            Summary = "Sign-in/register with Microsoft",
            Description = "Get a URL to redirect the user to, to let them sign-in/register with Microsoft. The user will be redirected back to the client with an authentication code once done."
        )]
        public async Task<ResponseModel<AuthenticationRedirectModel>> SignInWithMicrosoft(
            [Required, FromQuery(Name = "redirect_url")]
            [SwaggerParameter("The URL to redirect the user to once the operation is completed")]
            string? redirectUrl,

            [FromQuery(Name = "state")]
            [SwaggerParameter("A freely-defined value that will sent back to the client")]
            string? clientState
            )
        {
            return await SignInWithExternalService(redirectUrl!, clientState, async state => {
                return await _microsoft.GetAuthorizationRequestUrl(new[] {"user.read"})
                    .WithExtraQueryParameters(new Dictionary<string, string> {
                        {"state", state}
                    })
                    .ExecuteAsync();
            });
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
            return await SignInWithExternalServiceCallback(state, UserModel.UserType.Microsoft, async () => {
                var provider = new AuthorizationCodeProvider(_microsoft, new[] {"user.read"});
                await provider.GetAccessTokenByAuthorizationCode(code);

                var userInfo = await new GraphServiceClient(provider).Me
                    .Request()
                    .GetAsync(cancellationToken);

                return userInfo.Mail;
            });
        }

        public async Task<ResponseModel<AuthenticationRedirectModel>> SignInWithExternalService(string redirectUrl, string? clientState, Func<string, Task<Uri>> uriGetter)
        {
            var externalAuthModel = new ExternalAuthModel {
                RedirectUrl = redirectUrl!,
                State = clientState
            };

            var state = JsonConvert.SerializeObject(externalAuthModel, new JsonSerializerSettings {
                NullValueHandling = NullValueHandling.Ignore
            });

            var uri =  await uriGetter(state);

            return new ResponseModel<AuthenticationRedirectModel> {
                Data = new AuthenticationRedirectModel {
                    RedirectUrl = uri.AbsoluteUri
                }
            };
        }

        private async Task<RedirectResult> SignInWithExternalServiceCallback(string state, UserModel.UserType type, Func<Task<string>> emailGetter)
        {
            var authRequestBody = JsonConvert.DeserializeObject<ExternalAuthModel>(state);

            UriBuilder redirectUrl;
            try {
                redirectUrl = new UriBuilder(authRequestBody.RedirectUrl);
            } catch {
                throw new BadRequestHttpException();
            }

            var query = HttpUtility.ParseQueryString(redirectUrl.Query);

            if (authRequestBody.State != null)
                query["state"] = authRequestBody.State;

            try {
                var email = await emailGetter();

                var authResult = await _authService.AuthenticateExternalUserAsync(email, type);

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