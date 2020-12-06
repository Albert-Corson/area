using System;
using System.Web;
using Dashboard.API.Exceptions.Http;
using Imgur.API.Authentication.Impl;
using Imgur.API.Enums;
using Imgur.API.Models.Impl;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Configuration;

namespace Dashboard.API.Services.Services
{
    public class ImgurServiceService : IServiceService
    {
        public ImgurServiceService(IConfiguration configuration)
        {
            var imgurConf = configuration.GetSection("WidgetApiKeys").GetSection("Imgur");
            if (imgurConf == null)
                return;
            var clientId = imgurConf["ClientId"];
            var clientSecret = imgurConf["ClientSecret"];
            Client = new ImgurClient(clientId, clientSecret);
        }

        public ImgurClient? Client { get; }

        public string Name { get; } = "Imgur";

        public string? LogIn(HttpContext context)
        {
            if (Client == null)
                throw new InternalServerErrorHttpException();
            return new Imgur.API.Endpoints.Impl.OAuth2Endpoint(Client).GetAuthorizationUrl(OAuth2ResponseType.Token);
        }

        public void HandleLogInCallback(HttpContext context)
        {
            var uri = new Uri(context.Request.GetEncodedUrl());
            var queryParams = HttpUtility.ParseQueryString(uri.Fragment.Substring(1));
            // access_token
            // expires_in
            // token_type
            // refresh_token
            // account_username
            // account_id
            var accessToken = queryParams["access_token"];
            // Client.SetOAuth2Token(new OAuth2Token());
        }
    }
}
