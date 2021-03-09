using System.Collections.Generic;
using System.Threading.Tasks;
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

        public async Task<IEnumerable<WidgetCallResponseItemModel>> CallWidgetApiAsync(
            IEnumerable<ParamModel> widgetCallParams)
        {
            var sort = widgetCallParams.GetEnumValue<AccountGallerySortOrder>("sort");

            var accountEndpoint = new AccountEndpoint(Imgur.Client);
            var result = await accountEndpoint.GetAccountGalleryFavoritesAsync(sort: sort);

            return ImgurService.WidgetResponseItemsFromGallery(result);
        }
    }
}