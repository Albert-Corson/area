using System;
using Dashboard.API.Models.Services;
using Dashboard.API.Models.Table;
using Dashboard.API.Models.Table.Owned;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RedditSharp;

namespace Dashboard.API.Services.Services
{
    public class RedditServiceService : IServiceService
    {
        private readonly string? _clientId;

        private readonly string? _clientSecret;

        private readonly string? _redirectUri;

        public RedditServiceService(IConfiguration configuration)
        {
            var redditConf = configuration.GetSection("WidgetApiKeys").GetSection(Name);
            if (redditConf == null)
                return;
            _clientId = redditConf["ClientId"];
            _clientSecret = redditConf["ClientSecret"];
            _redirectUri = redditConf["RedirectUri"];
        }

        public string Name { get; } = "Reddit";

        public Uri? SignIn(HttpContext context, int userId)
        {
            if (_clientId == null || _clientSecret == null || _redirectUri == null)
                return null;

            var authProvider = new AuthProvider(_clientId, _clientSecret, _redirectUri);

            return new Uri(authProvider.GetAuthUrl(userId.ToString(), AuthProvider.Scope.read, true));
        }

        public int? GetUserIdFromCallbackContext(HttpContext context)
        {
            if (!context.Request.Query.TryGetValue("state", out var state) || !int.TryParse(state, out var userId))
                return null;
            return userId;
        }

        public void HandleSignInCallback(HttpContext context, int serviceId, UserModel user)
        {
            if (!context.Request.Query.TryGetValue("code", out var code))
                return;

            var authProvide = new AuthProvider(_clientId, _clientSecret, _redirectUri);

            var tokenHolder = new RedditAuthModel {
                AccessToken = authProvide.GetOAuthToken(code)
            };

            user.ServiceTokens?.Add(new UserServiceTokensModel {
                Json = tokenHolder.ToString(),
                ServiceId = serviceId
            });
        }

        public Reddit? ClientFromJsonTokens(string json)
        {
            try {
                var holder = JsonConvert.DeserializeObject<RedditAuthModel>(json);

                return new Reddit(holder.AccessToken);
            } catch {
                return null;
            }
        }
    }
}
