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
    public class ImgurFavoritesWidget : IWidget
    {
        public ImgurFavoritesWidget(ImgurService imgur)
        {
            Imgur = imgur;
        }

        private ImgurService Imgur { get; }

        public int Id { get; } = 2;

        public IEnumerable<WidgetCallResponseItemModel> CallWidgetApi(IEnumerable<ParamModel> widgetCallParams)
        {
            var sort = widgetCallParams.GetEnumValue<AccountGallerySortOrder>("sort");

            var task = new AccountEndpoint(Imgur.Client).GetAccountGalleryFavoritesAsync(sort: sort);
            task.Wait();

            if (!task.IsCompletedSuccessfully)
                throw new InternalServerErrorHttpException("Could not reach Imgur");

            return ImgurService.WidgetResponseItemsFromGallery(task.Result);
        }
    }
}