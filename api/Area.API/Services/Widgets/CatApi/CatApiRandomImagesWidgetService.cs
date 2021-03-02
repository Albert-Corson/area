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
    public class CatApiRandomImagesWidgetService : IWidgetService
    {
        private readonly string _apiKey;

        public CatApiRandomImagesWidgetService(IConfiguration configuration)
        {
            _apiKey = configuration[AuthConstants.CatApi.Key];
            Client = new CatClient();
        }

        private CatClient Client { get; }

        public string Name { get; } = "Random cat images";

        public void CallWidgetApi(IEnumerable<ParamModel> _,
            ref WidgetCallResponseModel response)
        {
            try {
                var request = new GetRequestBuilder()
                    .WithApiKey(_apiKey)
                    .WithResultsPerPage(50);

                var images = Client.GetImages(request);

                if (images == null)
                    throw new InternalServerErrorHttpException("Could not reach The Cat Api");

                response.Items = images.Select(image => new WidgetCallResponseItemModel {
                    Image = image.Url,
                    Link = image.SourceUrl
                });
            } catch {
                throw new InternalServerErrorHttpException();
            }
        }
    }
}