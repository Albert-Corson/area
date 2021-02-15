using System;
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
    public class ImgurGalleryWidgetService : IWidgetService
    {
        public ImgurGalleryWidgetService(ImgurServiceService imgur)
        {
            Imgur = imgur;
        }

        private ImgurServiceService Imgur { get; }

        public string Name { get; } = "Imgur public gallery";

        public void CallWidgetApi(IEnumerable<ParamModel> widgetCallParams,
            ref WidgetCallResponseModel response)
        {
            if (Imgur.Client == null)
                throw new InternalServerErrorHttpException();
            var galleryEndpoint = new GalleryEndpoint(Imgur.Client);

            var sectionStr = widgetCallParams.GetValue("section");
            if (!Enum.TryParse<GallerySection>(sectionStr, true, out var section))
                throw new BadRequestHttpException(
                    $"Query parameter `sort` has an invalid value `{sectionStr}`. Expected hot|top|user");

            var task = galleryEndpoint.GetGalleryAsync(section);
            task.Wait();
            if (!task.IsCompletedSuccessfully)
                throw new InternalServerErrorHttpException("Could not reach Imgur");

            response.Items = ImgurServiceService.WidgetResponseItemsFromGallery(task.Result);
        }
    }
}