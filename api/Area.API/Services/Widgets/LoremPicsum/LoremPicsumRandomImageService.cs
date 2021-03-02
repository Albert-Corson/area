using System;
using System.Collections.Generic;
using System.Linq;
using Area.API.Exceptions.Http;
using Area.API.Extensions;
using Area.API.Models;
using Area.API.Models.Table;
using RestSharp;

namespace Area.API.Services.Widgets.LoremPicsum
{
    public class LoremPicsumRandomImageService : IWidgetService
    {
        public string Name { get; } = "Lorem Picsum random Image";

        public void CallWidgetApi(IEnumerable<ParamModel> widgetCallParams,
            ref WidgetCallResponseModel response)
        {
            var width = widgetCallParams.GetValue<int>("width");
            var height = widgetCallParams.GetValue<int>("height");

            var url = $"https://picsum.photos/{width}/{height}";

            var client = new RestClient(url) {
                Timeout = 5000,
                FollowRedirects = false,
                ThrowOnAnyError = false
            };
            var request = new RestRequest(Method.GET);
            var restResponse = client.Execute(request);
            if (restResponse.ResponseStatus != ResponseStatus.Completed)
                throw new InternalServerErrorHttpException();

            var locationHeaderParameter = restResponse.Headers
                .FirstOrDefault(parameter =>
                    string.Compare(parameter.Name, "Location", StringComparison.OrdinalIgnoreCase) == 0
                    && parameter.Type == ParameterType.HttpHeader);

            if (locationHeaderParameter == null || !(locationHeaderParameter.Value is string location))
                throw new InternalServerErrorHttpException();

            response.Item = new WidgetCallResponseItemModel {
                Image = location,
                Link = location
            };
        }
    }
}