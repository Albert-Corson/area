using System;
using System.Threading.Tasks;
using Area.API.Constants;
using Area.API.Models.Services;
using Area.API.Models.Table.Owned;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SpotifyAPI.Web;

namespace Area.API.Services.Services
{
    public class SpotifyService : IService
    {
        private readonly string _clientId;

        private readonly string _clientSecret;

        private readonly Uri _redirectUri;

        public SpotifyClient? Client { get; private set; }

        public SpotifyService(IConfiguration configuration)
        {
            _clientId = configuration[AuthConstants.Spotify.ClientId];
            _clientSecret = configuration[AuthConstants.Spotify.ClientSecret];
            _redirectUri = new Uri(configuration[AuthConstants.Spotify.RedirectUri]);
        }

        public int Id { get; } = 3;

        public Task<Uri> GetSignInUrlAsync(string state)
        {
            var loginRequest = new LoginRequest(_redirectUri, _clientId, LoginRequest.ResponseType.Code) {
                Scope = new[] {
                    Scopes.UserModifyPlaybackState,
                    Scopes.UserTopRead,
                    Scopes.UserReadRecentlyPlayed,
                    Scopes.PlaylistReadCollaborative,
                    Scopes.PlaylistReadPrivate
                },
                State = state
            };
            return Task.FromResult(loginRequest.ToUri());
        }

        public async Task<string?> HandleSignInCallbackAsync(string code)
        {
            var tokenResponse = await new OAuthClient().RequestToken(
                new AuthorizationCodeTokenRequest(_clientId, _clientSecret, code, _redirectUri)
            );

            var tokensHolder = new SpotifyAuthModel {
                Scope = tokenResponse.Scope,
                AccessToken = tokenResponse.AccessToken,
                RefreshToken = tokenResponse.RefreshToken,
                ExpiresIn = tokenResponse.ExpiresIn,
                TokenType = tokenResponse.TokenType,
                CreatedAt = tokenResponse.CreatedAt
            };

            return tokensHolder.ToString();
        }

        public bool SignIn(UserServiceTokensModel tokens)
        {
            var oauth2 = OAuth2TokenFromJson(tokens.Json);

            if (oauth2 == null)
                return false;

            var clientConfig = SpotifyClientConfig
                .CreateDefault()
                .WithAuthenticator(new AuthorizationCodeAuthenticator(_clientId, _clientSecret, oauth2));

            Client = new SpotifyClient(clientConfig);

            return true;
        }

        private static AuthorizationCodeTokenResponse? OAuth2TokenFromJson(string json)
        {
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
                return tokens;
            } catch {
                return null;
            }
        }
    }
}