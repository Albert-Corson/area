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
    public class ImgurGalleryWidget : IWidget
    {
        public ImgurGalleryWidget(ImgurService imgur)
        {
            Imgur = imgur;
        }

        private ImgurService Imgur { get; }

        public int Id { get; } = 1;

        public async Task<IEnumerable<WidgetCallResponseItemModel>> CallWidgetApiAsync(
            IEnumerable<ParamModel> widgetCallParams)
        {
            if (Imgur.Client == null)
                throw new InternalServerErrorHttpException();
            var galleryEndpoint = new GalleryEndpoint(Imgur.Client);

            var section = widgetCallParams.GetEnumValue<GallerySection>("section");

            var result = await galleryEndpoint.GetGalleryAsync(section);

            return ImgurService.WidgetResponseItemsFromGallery(result);
        }
    }
}