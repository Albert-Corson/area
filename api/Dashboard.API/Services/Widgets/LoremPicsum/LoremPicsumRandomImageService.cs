using System;
using System.Linq;
using Dashboard.API.Exceptions.Http;
using Dashboard.API.Models.Response;
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
            var url = $"https://picsum.photos/{widgetCallParams.Integers["width"]}/{widgetCallParams.Integers["height"]}";

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
                .FirstOrDefault(parameter => {
                    return string.Compare(parameter.Name, "Location", StringComparison.OrdinalIgnoreCase) == 0
                           && parameter.Type == ParameterType.HttpHeader;
                });

            if (locationHeaderParameter != null && locationHeaderParameter.Value is string location) {
                return new ResponseModel<string> {
                    Data = location
                };
            }

            throw new InternalServerErrorHttpException();
        }
    }
}
