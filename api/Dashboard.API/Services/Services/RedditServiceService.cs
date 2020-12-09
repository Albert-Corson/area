using System;
using System.Text;
using Dashboard.API.Models.Services;
using Dashboard.API.Models.Table;
using Dashboard.API.Models.Table.Owned;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Reddit;
using RedditSharp;
using RestSharp;

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

            var client = new RestClient("https://www.reddit.com/api/v1/access_token") {
                Timeout = 5000,
                FollowRedirects = false,
                ThrowOnAnyError = false
            };

            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(_clientId + ":" + _clientSecret)));
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("grant_type", "authorization_code");
            request.AddParameter("code", code);
            request.AddParameter("redirect_uri", _redirectUri!);

            var response = client.Execute(request);

            if (response.ResponseStatus != ResponseStatus.Completed)
                return;

            user.ServiceTokens?.Add(new UserServiceTokensModel {
                Json = JsonConvert.DeserializeObject<RedditAuthModel>(response.Content).ToString(),
                ServiceId = serviceId
            });
        }

        public RedditClient? ClientFromJson(string json)
        {
            try {
                var holder = JsonConvert.DeserializeObject<RedditAuthModel>(json);

                var client = new RedditClient(_clientId, holder.RefreshToken, _clientSecret, holder.AccessToken);
                return client;
            } catch {
                return null;
            }
        }
    }
}
