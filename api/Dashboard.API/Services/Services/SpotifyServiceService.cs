using System;
using Dashboard.API.Models.Services;
using Dashboard.API.Models.Table;
using Dashboard.API.Models.Table.Owned;
using Dashboard.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SpotifyAPI.Web;

namespace Dashboard.API.Services.Services
{
    public class SpotifyServiceService : IServiceService
    {
        private readonly string? _clientId;

        private readonly string? _clientSecret;

        private readonly Uri? _redirectUri;

        public SpotifyServiceService(IConfiguration configuration)
        {
            var spotifyConf = configuration.GetSection("WidgetApiKeys").GetSection(Name);
            if (spotifyConf == null)
                return;
            _clientId = spotifyConf["ClientId"];
            _clientSecret = spotifyConf["ClientSecret"];
            _redirectUri = new Uri(spotifyConf["RedirectUri"]);
        }

        public string Name { get; } = "Spotify";

        public int? GetUserIdFromCallbackContext(HttpContext context)
        {
            if (!context.Request.Query.TryGetValue("state", out var state) || !int.TryParse(state, out var userId))
                return null;
            return userId;
        }

        public Uri? SignIn(HttpContext context, int userId)
        {
            if (_redirectUri == null || _clientId == null)
                return null;
            var loginRequest = new LoginRequest(_redirectUri, _clientId, LoginRequest.ResponseType.Code) {
                Scope = new[] {
                    Scopes.UserModifyPlaybackState,
                    Scopes.UserTopRead,
                    Scopes.UserReadRecentlyPlayed,
                    Scopes.PlaylistReadCollaborative,
                    Scopes.PlaylistReadPrivate
                },
                State = userId.ToString()
            };
            return loginRequest.ToUri();
        }

        public void HandleSignInCallback(HttpContext context, int serviceId, UserModel user)
        {
            if (_redirectUri == null || _clientId == null || _clientSecret == null)
                return;

            if (!context.Request.Query.TryGetValue("code", out var code))
                return;

            var task = new OAuthClient().RequestToken(
                new AuthorizationCodeTokenRequest(_clientId, _clientSecret, code, _redirectUri)
            );
            task.Wait();

            if (!task.IsCompletedSuccessfully)
                return;

            var tokensHolder = new SpotifyAuthModel {
                Scope = task.Result.Scope,
                AccessToken = task.Result.AccessToken,
                RefreshToken = task.Result.RefreshToken,
                ExpiresIn = task.Result.ExpiresIn,
                TokenType = task.Result.TokenType,
                CreatedAt = task.Result.CreatedAt
            };

            user.ServiceTokens?.Add(new UserServiceTokensModel {
                Json = tokensHolder.ToString(),
                ServiceId = serviceId
            });
        }

        public SpotifyClient? ClientFromJson(string json)
        {
            if (_clientId == null || _clientSecret == null)
                return null;

            try {
                var holder = JsonConvert.DeserializeObject<SpotifyAuthModel>(json);
                var tokens = new AuthorizationCodeTokenResponse {
                    Scope = holder.Scope!,
                    AccessToken = holder.AccessToken!,
                    RefreshToken = holder.RefreshToken!,
                    ExpiresIn = holder.ExpiresIn!.Value,
                    TokenType = holder.TokenType!,
                    CreatedAt = holder.CreatedAt!.Value
                };

                var clientConfig = SpotifyClientConfig
                    .CreateDefault()
                    .WithAuthenticator(new AuthorizationCodeAuthenticator(_clientId, _clientSecret, tokens));

                return new SpotifyClient(clientConfig);
            } catch {
                return null;
            }
        }
    }
}
