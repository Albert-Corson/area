using System;
using System.Collections.Generic;
using System.Linq;
using Dashboard.API.Exceptions.Http;
using Dashboard.API.Models;
using Dashboard.API.Models.Table;
using Dashboard.API.Models.Widgets;
using Dashboard.API.Services.Services;
using Imgur.API.Endpoints.Impl;
using Imgur.API.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.API.Services.Widgets.Imgur
{
    public class ImgurGalleryWidgetService : IWidgetService
    {
        public ImgurGalleryWidgetService(ImgurServiceService imgur)
        {
            Imgur = imgur;
        }

        private ImgurServiceService Imgur { get; }

        public string Name { get; } = "Imgur public gallery";

        public JsonResult CallWidgetApi(HttpContext context, UserModel user, WidgetModel widget, WidgetCallParameters widgetCallParams)
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
                throw new InternalServerErrorHttpException("Couldn't not reach Imgur's API");

            return new ResponseModel<List<ImgurGalleryItemModel>> {
                Data = ImgurServiceService.CoverImageListFromGallery(task.Result)
            };
        }
    }
}
