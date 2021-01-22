using System;
using System.Linq;
using Area.API.Exceptions.Http;
using Area.API.Models;
using Microsoft.AspNetCore.Http;
using RestSharp;

namespace Area.API.Services.Widgets.LoremPicsum
{
    public class LoremPicsumRandomImageService : IWidgetService
    {
        public string Name { get; } = "Lorem Picsum random Image";

        public void CallWidgetApi(HttpContext context, WidgetCallParameters widgetCallParams, ref WidgetCallResponseModel response)
        {
            var width = widgetCallParams.Integers["width"];
            var height = widgetCallParams.Integers["height"];

            if (height == null || width == null)
                throw new BadRequestHttpException();

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
                .FirstOrDefault(parameter => string.Compare(parameter.Name, "Location", StringComparison.OrdinalIgnoreCase) == 0
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
