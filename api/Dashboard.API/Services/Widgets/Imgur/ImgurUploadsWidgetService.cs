using System.Linq;
using Dashboard.API.Exceptions.Http;
using Dashboard.API.Models;
using Dashboard.API.Models.Table.Owned;
using Dashboard.API.Services.Services;
using Imgur.API.Endpoints.Impl;
using Imgur.API.Models.Impl;
using Microsoft.AspNetCore.Http;

namespace Dashboard.API.Services.Widgets.Imgur
{
    public class ImgurUploadsWidgetService : IWidgetService
    {
        private OAuth2Token? _oAuth2Token;

        public ImgurUploadsWidgetService(ImgurServiceService imgur)
        {
            Imgur = imgur;
        }

        private ImgurServiceService Imgur { get; }

        public bool ValidateServiceAuth(UserServiceTokensModel serviceTokens)
        {
            _oAuth2Token = ImgurServiceService.ImgurOAuth2TokenFromJson(serviceTokens.Json!);
            return _oAuth2Token != null;
        }

        public string Name { get; } = "Imgur uploads";

        public void CallWidgetApi(HttpContext context, WidgetCallParameters widgetCallParams, ref WidgetCallResponseModel response)
        {
            if (Imgur.Client == null || _oAuth2Token == null)
                throw new InternalServerErrorHttpException();

            Imgur.Client.SetOAuth2Token(_oAuth2Token);

            var task = new AccountEndpoint(Imgur.Client).GetAlbumsAsync();
            task.Wait();

            if (!task.IsCompletedSuccessfully)
                throw new InternalServerErrorHttpException("Couldn't not reach Imgur's API");

            response.Items = task.Result
                .Select(item => new WidgetCallResponseItemModel {
                    Image = item.Images.FirstOrDefault()?.Link,
                    Header = item.Title,
                    Content = item.Description,
                    Link = item.Link
                })
                .Where(responseItem => responseItem.Image != null);
        }
    }
}
