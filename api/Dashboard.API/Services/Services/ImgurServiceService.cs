using System;
using System.Collections.Generic;
using Dashboard.API.Exceptions.Http;
using Dashboard.API.Models.Services;
using Dashboard.API.Models.Table;
using Dashboard.API.Models.Table.Owned;
using Dashboard.API.Models.Widgets;
using Imgur.API.Authentication.Impl;
using Imgur.API.Enums;
using Imgur.API.Models;
using Imgur.API.Models.Impl;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Dashboard.API.Services.Services
{
    public class ImgurServiceService : IServiceService
    {
        public ImgurServiceService(IConfiguration configuration)
        {
            var imgurConf = configuration.GetSection("WidgetApiKeys").GetSection(Name);
            if (imgurConf == null)
                return;
            var clientId = imgurConf["ClientId"];
            var clientSecret = imgurConf["ClientSecret"];
            Client = new ImgurClient(clientId, clientSecret);
        }

        public ImgurClient? Client { get; }

        public string Name { get; } = "Imgur";

        public Uri? SignIn(HttpContext context, int userId)
        {
            if (Client == null)
                throw new InternalServerErrorHttpException();
            var oAuth2Endpoint = new Imgur.API.Endpoints.Impl.OAuth2Endpoint(Client);
            return new Uri(oAuth2Endpoint.GetAuthorizationUrl(OAuth2ResponseType.Code, userId.ToString()));
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

            try {
                var task = new Imgur.API.Endpoints.Impl.OAuth2Endpoint(Client).GetTokenByCodeAsync(code);
                task.Wait();
                if (!task.IsCompletedSuccessfully || task.Result == null)
                    return;
                var tokensHolder = new ImgurAuthModel {
                    AccessToken = task.Result.AccessToken,
                    TokenType = task.Result.TokenType,
                    RefreshToken = task.Result.RefreshToken,
                    AccountUsername = task.Result.AccountUsername,
                    AccountId = task.Result.AccountId,
                    ExpiresIn = task.Result.ExpiresIn
                };
                user.ServiceTokens?.Add(new UserServiceTokensModel {
                    ServiceId = serviceId,
                    Json = tokensHolder.ToString()
                });
            } catch {
                // ignored
            }
        }

        public static OAuth2Token? ImgurOAuth2TokenFromJson(string json)
        {
            try {
                var holder = JsonConvert.DeserializeObject<ImgurAuthModel>(json);
                return new OAuth2Token(
                    holder.AccessToken,
                    holder.RefreshToken,
                    holder.TokenType,
                    holder.AccountId,
                    holder.AccountUsername,
                    holder.ExpiresIn!.Value);
            } catch {
                return null;
            }
        }

        public static List<ImgurGalleryItemModel> CoverImageListFromGallery(IEnumerable<IGalleryItem> gallery)
        {
            List<ImgurGalleryItemModel> imageList = new List<ImgurGalleryItemModel>();

            foreach (var galleryItem in gallery) {
                ImgurGalleryItemModel responseItem;
                switch (galleryItem) {
                    case GalleryAlbum album:
                        responseItem = new ImgurGalleryItemModel(album);
                        break;
                    case GalleryImage image:
                        responseItem = new ImgurGalleryItemModel(image);
                        break;
                    default:
                        continue;
                }

                if (responseItem.Cover == null)
                    continue;
                imageList.Add(responseItem);
            }

            return imageList;
        }
    }
}
