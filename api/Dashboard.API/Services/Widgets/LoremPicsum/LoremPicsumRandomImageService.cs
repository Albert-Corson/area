using System;
using System.Linq;
using Dashboard.API.Exceptions.Http;
using Dashboard.API.Models;
using Dashboard.API.Models.Table;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestSharp;

namespace Dashboard.API.Services.Widgets.LoremPicsum
{
    public class LoremPicsumRandomImageService : IWidgetService
    {
        public string Name { get; } = "Lorem Picsum random Image";

        public JsonResult CallWidgetApi(HttpContext context, UserModel user, WidgetModel widget, WidgetCallParameters widgetCallParams)
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
            IRestResponse response = client.Execute(request);
            if (response.ResponseStatus != ResponseStatus.Completed)
                throw new InternalServerErrorHttpException();

            var locationHeaderParameter = response.Headers
                .FirstOrDefault(parameter => string.Compare(parameter.Name, "Location", StringComparison.OrdinalIgnoreCase) == 0
                                             && parameter.Type == ParameterType.HttpHeader);

            if (locationHeaderParameter != null && locationHeaderParameter.Value is string location) {
                return new ResponseModel<string> {
                    Data = location
                };
            }

            throw new InternalServerErrorHttpException();
        }
    }
}
