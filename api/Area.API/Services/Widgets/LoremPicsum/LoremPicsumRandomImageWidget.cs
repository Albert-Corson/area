using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Area.API.Exceptions.Http;
using Area.API.Extensions;
using Area.API.Models;
using Area.API.Models.Table;
using RestSharp;

namespace Area.API.Services.Widgets.LoremPicsum
{
    public class LoremPicsumRandomImageWidget : IWidget
    {
        public int Id { get; } = 4;

        public async Task<IEnumerable<WidgetCallResponseItemModel>> CallWidgetApiAsync(
            IEnumerable<ParamModel> widgetCallParams)
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
            var restResponse = await client.ExecuteAsync(request);
            if (restResponse.ResponseStatus != ResponseStatus.Completed)
                throw new InternalServerErrorHttpException();

            var locationHeaderParameter = restResponse.Headers
                .FirstOrDefault(parameter =>
                    string.Compare(parameter.Name, "Location", StringComparison.OrdinalIgnoreCase) == 0
                    && parameter.Type == ParameterType.HttpHeader);

            if (locationHeaderParameter == null || !(locationHeaderParameter.Value is string location))
                throw new InternalServerErrorHttpException();

            return new[] {
                new WidgetCallResponseItemModel {
                    Image = location,
                    Link = location
                }
            };
        }
    }
}