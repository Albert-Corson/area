using System.Collections.Generic;
using System.Linq;
using Dashboard.API.Exceptions.Http;
using Dashboard.API.Models.Services.Imgur;
using Dashboard.API.Models.Table.Owned;
using Dashboard.API.Repositories;
using Imgur.API.Authentication.Impl;
using Imgur.API.Enums;
using Imgur.API.Models;
using Imgur.API.Models.Impl;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Dashboard.API.Services.Services
{
    public class ImgurServiceService : IServiceService
    {
        private readonly DatabaseRepository _database;

        public ImgurServiceService(IConfiguration configuration, DatabaseRepository database)
        {
            _database = database;
            var imgurConf = configuration.GetSection("WidgetApiKeys").GetSection("Imgur");
            if (imgurConf == null)
                return;
            var clientId = imgurConf["ClientId"];
            var clientSecret = imgurConf["ClientSecret"];
            Client = new ImgurClient(clientId, clientSecret);
        }

        public ImgurClient? Client { get; }

        public string Name { get; } = "Imgur";

        public string? SignIn(HttpContext context)
        {
            var userId = AuthService.GetUserIdFromPrincipal(context.User);

            if (userId == null)
                return null;
            if (Client == null)
                throw new InternalServerErrorHttpException();
            return new Imgur.API.Endpoints.Impl.OAuth2Endpoint(Client).GetAuthorizationUrl(OAuth2ResponseType.Code, userId.ToString());
        }

        public void HandleSignInCallback(HttpContext context, int serviceId)
        {
            if (!context.Request.Query.TryGetValue("code", out var code))
                return;
            if (!context.Request.Query.TryGetValue("state", out var state) || !int.TryParse(state, out var userId))
                return;
            var user = _database.Users
                .Include(model => model.ServiceTokens)
                .FirstOrDefault(model => model.Id == userId);
            if (user == null)
                return;

            try {
                var task = new Imgur.API.Endpoints.Impl.OAuth2Endpoint(Client).GetTokenByCodeAsync(code);
                task.Wait();
                if (!task.IsCompletedSuccessfully || task.Result == null)
                    return;
                var tokens = new OAuth2TokensModel {
                    AccessToken = task.Result.AccessToken,
                    TokenType = task.Result.TokenType,
                    RefreshToken = task.Result.RefreshToken,
                    AccountUsername = task.Result.AccountUsername,
                    AccountId = task.Result.AccountId,
                    ExpiresIn = task.Result.ExpiresIn
                };
                var oldToken = user.ServiceTokens?.FirstOrDefault(model => model.ServiceId == serviceId);
                if (oldToken != null) {
                    user.ServiceTokens!.Remove(oldToken);
                }
                user.ServiceTokens?.Add(new UserServiceTokensModel {
                    ServiceId = serviceId,
                    Json = tokens.ToString()
                });
            } catch {
                // ignored
            }
        }

        public static OAuth2Token? ImgurOAuth2TokenFromJson(string json)
        {
            try {
                var holder = JsonConvert.DeserializeObject<OAuth2TokensModel>(json);
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
