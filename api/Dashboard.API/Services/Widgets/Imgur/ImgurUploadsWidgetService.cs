using System.Collections.Generic;
using System.Linq;
using Dashboard.API.Exceptions.Http;
using Dashboard.API.Models;
using Dashboard.API.Models.Table.Owned;
using Dashboard.API.Models.Widgets;
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

            var items = new List<WidgetCallResponseItemModel>();
            foreach (var album in task.Result)
            {
                var imageTask = new AlbumEndpoint(Imgur.Client).GetAlbumImagesAsync(album.Id);
                imageTask.Wait();
                if (!imageTask.IsCompletedSuccessfully)
                    continue;
                var imageLink = imageTask.Result.FirstOrDefault()?.Link;
                if (imageLink == null)
                    continue;
                items.Add(new WidgetCallResponseItemModel {
                    Image = imageLink,
                    Header = album.Title,
                    Content = album.Description,
                    Link = album.Link
                });
            }

            response.Items = items;
        }
    }
}
