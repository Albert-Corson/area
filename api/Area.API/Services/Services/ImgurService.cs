using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Area.API.Constants;
using Area.API.Exceptions.Http;
using Area.API.Models;
using Area.API.Models.Services;
using Area.API.Models.Table.Owned;
using Imgur.API.Authentication.Impl;
using Imgur.API.Endpoints.Impl;
using Imgur.API.Enums;
using Imgur.API.Models;
using Imgur.API.Models.Impl;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Area.API.Services.Services
{
    public class ImgurService : IService
    {
        public ImgurService(IConfiguration configuration)
        {
            var clientId = configuration[AuthConstants.Imgur.ClientId];
            var clientSecret = configuration[AuthConstants.Imgur.ClientSecret];
            Client = new ImgurClient(clientId, clientSecret);
        }

        public ImgurClient Client { get; }

        public int Id { get; } = 1;

        public Task<Uri> GetSignInUrlAsync(string state)
        {
            var oAuth2Endpoint = new OAuth2Endpoint(Client);
            var redirectUrl = oAuth2Endpoint.GetAuthorizationUrl(OAuth2ResponseType.Code, HttpUtility.UrlEncode(state));
            return Task.FromResult(new Uri(redirectUrl));
        }

        public async Task<string?> HandleSignInCallbackAsync(string code)
        {
            try {
                var auth2Token = await new OAuth2Endpoint(Client).GetTokenByCodeAsync(code);

                var tokensHolder = new ImgurAuthModel {
                    AccessToken = auth2Token.AccessToken,
                    TokenType = auth2Token.TokenType,
                    RefreshToken = auth2Token.RefreshToken,
                    AccountUsername = auth2Token.AccountUsername,
                    AccountId = auth2Token.AccountId,
                    ExpiresIn = auth2Token.ExpiresIn
                };
                return tokensHolder.ToString();
            } catch {
                return null;
            }
        }

        public bool SignIn(UserServiceTokensModel tokens)
        {
            var oAuth2Token = OAuth2TokenFromJson(tokens.Json);

            if (oAuth2Token == null)
                return false;
            Client.SetOAuth2Token(oAuth2Token);
            return true;
        }

        private static OAuth2Token? OAuth2TokenFromJson(string json)
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

        public static List<WidgetCallResponseItemModel> WidgetResponseItemsFromGallery(
            IEnumerable<IGalleryItem> gallery)
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