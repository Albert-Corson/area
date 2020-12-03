using System.Collections.Generic;
using System.Linq;
using Dashboard.API.Exceptions.Http;
using Dashboard.API.Models.Response;
using Dashboard.API.Models.Services.Imgur;
using Dashboard.API.Models.Table;
using Dashboard.API.Models.Table.Owned;
using Imgur.API.Authentication.Impl;
using Imgur.API.Enums;
using Imgur.API.Models.Impl;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Dashboard.API.Services.Widgets
{
    public class ImgurGalleryWidgetService : IWidgetService
    {
        private const string Name = "Imgur public gallery";

        private readonly ImgurClient? _imgurClient;

        private readonly IDictionary<string, GallerySection> _gallerySections = new Dictionary<string, GallerySection> {
            {"hot", GallerySection.Hot},
            {"top", GallerySection.Top},
            {"user", GallerySection.User},
        };

        public ImgurGalleryWidgetService(IConfiguration configuration)
        {
            var imgurConf = configuration.GetSection("WidgetApiKeys").GetSection("Imgur");
            if (imgurConf == null)
                return;
            var clientId = imgurConf["ClientId"];
            var clientSecret = imgurConf["ClientSecret"];
            _imgurClient = new ImgurClient(clientId, clientSecret);
        }

        public string GetWidgetName()
        {
            return Name;
        }

        public JsonResult CallWidgetApi(HttpContext context, UserModel user, WidgetModel widget, WidgetCallParameters widgetCallCallParams, UserServiceTokensModel? serviceTokens = null)
        {
            if (_imgurClient == null)
                throw new InternalServerErrorHttpException();
            var galleryEndpoint = new Imgur.API.Endpoints.Impl.GalleryEndpoint(_imgurClient);

            var sectionStr = widgetCallCallParams.Strings["section"].ToLower();

            if (!_gallerySections.TryGetValue(sectionStr, out var section))
                section = _gallerySections.First().Value;

            var task = galleryEndpoint.GetGalleryAsync(
                section: section,
                page: widgetCallCallParams.Integers["page"]);
            task.Wait();
            if (!task.IsCompletedSuccessfully)
                throw new InternalServerErrorHttpException("Couldn't not reach Imgur's API");

            List<ImgurGalleryItemModel> data = new List<ImgurGalleryItemModel>();
            foreach (var galleryItem in task.Result) {
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
                data.Add(responseItem);
            }

            return new ResponseModel<List<ImgurGalleryItemModel>> {
                Data = data
            };
        }
    }
}
