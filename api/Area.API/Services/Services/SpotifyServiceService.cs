using System;
using Area.API.Models.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SpotifyAPI.Web;

namespace Area.API.Services.Services
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

        public Uri? SignIn(int userId)
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

        public string? HandleSignInCallback(HttpContext context)
        {
            if (_redirectUri == null || _clientId == null || _clientSecret == null)
                return null;

            if (!context.Request.Query.TryGetValue("code", out var code))
                return null;

            var task = new OAuthClient().RequestToken(
                new AuthorizationCodeTokenRequest(_clientId, _clientSecret, code, _redirectUri)
            );
            task.Wait();

            if (!task.IsCompletedSuccessfully)
                return null;

            var tokensHolder = new SpotifyAuthModel {
                Scope = task.Result.Scope,
                AccessToken = task.Result.AccessToken,
                RefreshToken = task.Result.RefreshToken,
                ExpiresIn = task.Result.ExpiresIn,
                TokenType = task.Result.TokenType,
                CreatedAt = task.Result.CreatedAt
            };

            return tokensHolder.ToString();
        }

        public SpotifyClient? ClientFromJson(string json)
        {
            if (_clientId == null || _clientSecret == null)
                return null;

            try {
                var holder = JsonConvert.DeserializeObject<SpotifyAuthModel>(json);
                var tokens = new AuthorizationCodeTokenResponse {
                    Scope = holder.Scope,
                    AccessToken = holder.AccessToken,
                    RefreshToken = holder.RefreshToken,
                    ExpiresIn = holder.ExpiresIn,
                    TokenType = holder.TokenType,
                    CreatedAt = holder.CreatedAt
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