using System;
using System.Collections.Generic;
using System.Linq;
using Area.API.Exceptions.Http;
using Area.API.Models;
using Area.API.Models.Services;
using Imgur.API.Authentication.Impl;
using Imgur.API.Enums;
using Imgur.API.Models;
using Imgur.API.Models.Impl;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Area.API.Services.Services
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

        public string? HandleSignInCallback(HttpContext context)
        {
            if (!context.Request.Query.TryGetValue("code", out var code))
                return null;

            try {
                var task = new Imgur.API.Endpoints.Impl.OAuth2Endpoint(Client).GetTokenByCodeAsync(code);
                task.Wait();
                if (!task.IsCompletedSuccessfully || task.Result == null)
                    return null;
                var tokensHolder = new ImgurAuthModel {
                    AccessToken = task.Result.AccessToken,
                    TokenType = task.Result.TokenType,
                    RefreshToken = task.Result.RefreshToken,
                    AccountUsername = task.Result.AccountUsername,
                    AccountId = task.Result.AccountId,
                    ExpiresIn = task.Result.ExpiresIn
                };
                return tokensHolder.ToString();
            } catch {
                return null;
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
                    holder.ExpiresIn);
            } catch {
                return null;
            }
        }

        public static List<WidgetCallResponseItemModel> WidgetResponseItemsFromGallery(IEnumerable<IGalleryItem> gallery)
        {
            List<WidgetCallResponseItemModel> imageList = new List<WidgetCallResponseItemModel>();

            foreach (var galleryItem in gallery) {
                WidgetCallResponseItemModel responseItem = new WidgetCallResponseItemModel();
                switch (galleryItem) {
                    case GalleryAlbum album:
                        responseItem.Image = album.Images.FirstOrDefault()?.Link;
                        responseItem.Header = album.Title;
                        responseItem.Content = album.Description;
                        responseItem.Link = album.Link;
                        break;
                    case GalleryImage image:
                        responseItem.Image = image.Link;
                        responseItem.Header = image.Title;
                        responseItem.Content = image.Description;
                        responseItem.Link = image.Link;
                        break;
                    default:
                        continue;
                }

                if (responseItem.Image == null)
                    continue;
                imageList.Add(responseItem);
            }

            return imageList;
        }
    }
}
