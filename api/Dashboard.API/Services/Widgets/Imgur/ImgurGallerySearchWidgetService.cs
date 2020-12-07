using System;
using System.Collections.Generic;
using Dashboard.API.Exceptions.Http;
using Dashboard.API.Models.Response;
using Dashboard.API.Models.Services.Imgur;
using Dashboard.API.Models.Table;
using Dashboard.API.Services.Services;
using Imgur.API.Endpoints.Impl;
using Imgur.API.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.API.Services.Widgets.Imgur
{
    public class ImgurGallerySearchWidgetService : IWidgetService
    {
        public ImgurGallerySearchWidgetService(ImgurServiceService imgur)
        {
            Imgur = imgur;
        }

        private ImgurServiceService Imgur { get; }

        public string Name { get; } = "Imgur gallery search";

        public JsonResult CallWidgetApi(HttpContext context, UserModel user, WidgetModel widget, WidgetCallParameters widgetCallParams)
        {
            if (Imgur.Client == null)
                throw new InternalServerErrorHttpException();
            var galleryEndpoint = new GalleryEndpoint(Imgur.Client);

            var sortStr = widgetCallParams.Strings["sort"];
            if (!Enum.TryParse(typeof(GallerySortOrder), sortStr, true, out var sort))
                throw new BadRequestHttpException($"Query parameter `sort` has an invalid value `{sortStr}` but expected time|viral|top");

            if (!widgetCallParams.Undefined.TryGetValue("query", out var query))
                throw new BadRequestHttpException("Query parameter `query` is missing");

            var task = galleryEndpoint.SearchGalleryAsync(query, sort as GallerySortOrder? ?? GallerySortOrder.Time);
            task.Wait();
            if (!task.IsCompletedSuccessfully)
                throw new InternalServerErrorHttpException("Couldn't not reach Imgur's API");

            return new ResponseModel<List<ImgurGalleryItemModel>> {
                Data = ImgurServiceService.CoverImageListFromGallery(task.Result)
            };
        }
    }
}
