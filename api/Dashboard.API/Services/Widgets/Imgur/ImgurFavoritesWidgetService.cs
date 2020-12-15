using Dashboard.API.Exceptions.Http;
using Dashboard.API.Models;
using Dashboard.API.Models.Table.Owned;
using Dashboard.API.Services.Services;
using Imgur.API.Endpoints.Impl;
using Imgur.API.Enums;
using Imgur.API.Models.Impl;
using Microsoft.AspNetCore.Http;

namespace Dashboard.API.Services.Widgets.Imgur
{
    public class ImgurFavoritesWidgetService : IWidgetService
    {
        public ImgurFavoritesWidgetService(ImgurServiceService imgur)
        {
            Imgur = imgur;
        }

        private ImgurServiceService Imgur { get; }

        private OAuth2Token? _oAuth2Token;

        public bool ValidateServiceAuth(UserServiceTokensModel serviceTokens)
        {
            _oAuth2Token = ImgurServiceService.ImgurOAuth2TokenFromJson(serviceTokens.Json!);
            return _oAuth2Token != null;
        }

        public string Name { get; } = "Imgur favorites";

        public void CallWidgetApi(HttpContext context, WidgetCallParameters widgetCallParams, ref WidgetCallResponseModel response)
        {
            if (Imgur.Client == null || _oAuth2Token == null)
                throw new InternalServerErrorHttpException();

            Imgur.Client.SetOAuth2Token(_oAuth2Token);

            var sort = widgetCallParams.Strings["sort"] == "newest" ? AccountGallerySortOrder.Newest : AccountGallerySortOrder.Oldest;

            var task = new AccountEndpoint(Imgur.Client).GetAccountGalleryFavoritesAsync(sort: sort);
            task.Wait();

            if (!task.IsCompletedSuccessfully)
                throw new InternalServerErrorHttpException("Couldn't not reach Imgur's API");

            response.Items = ImgurServiceService.WidgetResponseItemsFromGallery(task.Result);
        }
    }
}
