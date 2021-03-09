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
    public class ImgurGalleryWidget : IWidget
    {
        public ImgurGalleryWidget(ImgurService imgur)
        {
            Imgur = imgur;
        }

        private ImgurService Imgur { get; }

        public int Id { get; } = 1;

        public IEnumerable<WidgetCallResponseItemModel> CallWidgetApi(IEnumerable<ParamModel> widgetCallParams)
        {
            if (Imgur.Client == null)
                throw new InternalServerErrorHttpException();
            var galleryEndpoint = new GalleryEndpoint(Imgur.Client);

            var section = widgetCallParams.GetEnumValue<GallerySection>("section");

            var task = galleryEndpoint.GetGalleryAsync(section);
            task.Wait();
            if (!task.IsCompletedSuccessfully)
                throw new InternalServerErrorHttpException("Could not reach Imgur");

            return ImgurService.WidgetResponseItemsFromGallery(task.Result);
        }
    }
}