using System;
using Area.API.Exceptions.Http;
using Area.API.Models;
using Area.API.Services.Services;
using Imgur.API.Endpoints.Impl;
using Imgur.API.Enums;
using Microsoft.AspNetCore.Http;

namespace Area.API.Services.Widgets.Imgur
{
    public class ImgurGallerySearchWidgetService : IWidgetService
    {
        public ImgurGallerySearchWidgetService(ImgurServiceService imgur)
        {
            Imgur = imgur;
        }

        private ImgurServiceService Imgur { get; }

        public string Name { get; } = "Imgur gallery search";

        public void CallWidgetApi(HttpContext context, WidgetCallParameters widgetCallParams, ref WidgetCallResponseModel response)
        {
            if (Imgur.Client == null)
                throw new InternalServerErrorHttpException();
            var galleryEndpoint = new GalleryEndpoint(Imgur.Client);

            var sortStr = widgetCallParams.Strings["sort"];
            if (!Enum.TryParse<GallerySortOrder>(sortStr, true, out var sort))
                throw new BadRequestHttpException($"Query parameter `sort` has an invalid value `{sortStr}`. Expected time|viral|top");

            var task = galleryEndpoint.SearchGalleryAsync(widgetCallParams.Strings["query"], sort);
            task.Wait();

            if (!task.IsCompletedSuccessfully)
                throw new InternalServerErrorHttpException("Could not reach Imgur");

            response.Items = ImgurServiceService.WidgetResponseItemsFromGallery(task.Result);
        }
    }
}
