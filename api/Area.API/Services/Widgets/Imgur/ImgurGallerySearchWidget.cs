using System.Collections.Generic;
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

        public IEnumerable<WidgetCallResponseItemModel> CallWidgetApi(IEnumerable<ParamModel> widgetCallParams)
        {
            if (Imgur.Client == null)
                throw new InternalServerErrorHttpException();
            var galleryEndpoint = new GalleryEndpoint(Imgur.Client);

            var sort = widgetCallParams.GetEnumValue<GallerySortOrder>("sort");

            var task = galleryEndpoint.SearchGalleryAsync(widgetCallParams.GetValue("query"), sort);
            task.Wait();

            if (!task.IsCompletedSuccessfully)
                throw new InternalServerErrorHttpException("Could not reach Imgur");

            return ImgurService.WidgetResponseItemsFromGallery(task.Result);
        }
    }
}