using System.Collections.Generic;
using System.Threading.Tasks;
using Area.API.Exceptions.Http;
using Area.API.Extensions;
using Area.API.Models;
using Area.API.Models.Table;
using Area.API.Services.Services;
using Imgur.API.Endpoints.Impl;
using Imgur.API.Enums;

namespace Area.API.Services.Widgets.Imgur
{
    public class ImgurGallerySearchWidget : IWidget
    {
        public ImgurGallerySearchWidget(ImgurService imgur)
        {
            Imgur = imgur;
        }

        private ImgurService Imgur { get; }

        public int Id { get; } = 5;

        public async Task<IEnumerable<WidgetCallResponseItemModel>> CallWidgetApiAsync(
            IEnumerable<ParamModel> widgetCallParams)
        {
            if (Imgur.Client == null)
                throw new InternalServerErrorHttpException();
            var galleryEndpoint = new GalleryEndpoint(Imgur.Client);

            var sort = widgetCallParams.GetEnumValue<GallerySortOrder>("sort");
            var query = widgetCallParams.GetValue("query");

            if (string.IsNullOrWhiteSpace(query))
                throw new BadRequestHttpException($"Parameter `{nameof(query)}` must have a value");
            var result = await galleryEndpoint.SearchGalleryAsync(query, sort);

            return ImgurService.WidgetResponseItemsFromGallery(result);
        }
    }
}