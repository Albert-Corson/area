using System.Collections.Generic;
using System.Linq;
using Area.API.Exceptions.Http;
using Area.API.Models;
using Area.API.Models.Table;
using Area.API.Models.Table.Owned;
using Area.API.Services.Services;
using Imgur.API.Endpoints.Impl;
using Imgur.API.Models.Impl;

namespace Area.API.Services.Widgets.Imgur
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

        public void CallWidgetApi(IEnumerable<ParamModel> _,
            ref WidgetCallResponseModel response)
        {
            if (Imgur.Client == null || _oAuth2Token == null)
                throw new InternalServerErrorHttpException();

            Imgur.Client.SetOAuth2Token(_oAuth2Token);

            var task = new AccountEndpoint(Imgur.Client).GetAlbumsAsync();
            task.Wait();

            if (!task.IsCompletedSuccessfully)
                throw new InternalServerErrorHttpException("Could not reach Imgur");

            var items = new List<WidgetCallResponseItemModel>();
            foreach (var album in task.Result) {
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