using System;
using Area.API.Exceptions.Http;
using Area.API.Models;
using Area.API.Services.Services;
using Imgur.API.Endpoints.Impl;
using Imgur.API.Enums;
using Microsoft.AspNetCore.Http;

namespace Area.API.Services.Widgets.Imgur
{
    public class ImgurGalleryWidgetService : IWidgetService
    {
        public ImgurGalleryWidgetService(ImgurServiceService imgur)
        {
            Imgur = imgur;
        }

        private ImgurServiceService Imgur { get; }

        public string Name { get; } = "Imgur public gallery";

        public void CallWidgetApi(HttpContext context, WidgetCallParameters widgetCallParams, ref WidgetCallResponseModel response)
        {
            if (Imgur.Client == null)
                throw new InternalServerErrorHttpException();
            var galleryEndpoint = new GalleryEndpoint(Imgur.Client);

            var sectionStr = widgetCallParams.Strings["section"];
            if (!Enum.TryParse<GallerySection>(sectionStr, true, out var section))
                throw new BadRequestHttpException($"Query parameter `sort` has an invalid value `{sectionStr}`. Expected hot|top|user");

            var task = galleryEndpoint.GetGalleryAsync(section);
            task.Wait();
            if (!task.IsCompletedSuccessfully)
                throw new InternalServerErrorHttpException("Could not reach Imgur");

            response.Items = ImgurServiceService.WidgetResponseItemsFromGallery(task.Result);
        }
    }
}
