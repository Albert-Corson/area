using System.Collections.Generic;
using System.Linq;
using Area.API.Constants;
using Area.API.Exceptions.Http;
using Area.API.Models;
using Area.API.Models.Table;
using CatApiWrapper;
using CatApiWrapper.RequestBuilders;
using Microsoft.Extensions.Configuration;

namespace Area.API.Services.Widgets.CatApi
{
    public class CatApiRandomImagesWidget : IWidget
    {
        private readonly string _apiKey;

        public CatApiRandomImagesWidget(IConfiguration configuration)
        {
            _apiKey = configuration[AuthConstants.CatApi.Key];
            Client = new CatClient();
        }

        private CatClient Client { get; }

        public int Id { get; } = 11;

        public IEnumerable<WidgetCallResponseItemModel> CallWidgetApi(IEnumerable<ParamModel> _)
        {
            try {
                var request = new GetRequestBuilder()
                    .WithApiKey(_apiKey)
                    .WithResultsPerPage(50);

                var images = Client.GetImages(request);

                if (images == null)
                    throw new InternalServerErrorHttpException("Could not reach The Cat Api");

                return images.Select(image => new WidgetCallResponseItemModel {
                    Image = image.Url,
                    Link = image.SourceUrl
                });
            } catch {
                throw new InternalServerErrorHttpException();
            }
        }
    }
}