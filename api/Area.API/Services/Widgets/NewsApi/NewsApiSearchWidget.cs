using System;
using System.Collections.Generic;
using System.Linq;
using Area.API.Constants;
using Area.API.Exceptions.Http;
using Area.API.Extensions;
using Area.API.Models;
using Area.API.Models.Table;
using Area.API.Models.Widgets;
using Area.API.Services.Services;
using Microsoft.Extensions.Configuration;
using NewsAPI;
using NewsAPI.Constants;
using NewsAPI.Models;

namespace Area.API.Services.Widgets.NewsApi
{
    public class NewsApiSearchWidget : IWidget
    {
        private readonly NewsApiClient? _client;

        public NewsApiSearchWidget(IConfiguration configuration)
        {
            var apiKey = configuration[AuthConstants.NewsApi.Key];

            if (apiKey != null)
                _client = new NewsApiClient(apiKey);
        }

        public int Id { get; } = 10;

        public IEnumerable<WidgetCallResponseItemModel> CallWidgetApi(IEnumerable<ParamModel> widgetCallParams)
        {
            var everythingRequest = new EverythingRequest {
                From = DateTime.UtcNow.Subtract(TimeSpan.FromDays(21)),
                Q = widgetCallParams.GetValue("query"),
                Language = widgetCallParams.GetEnumValue<Languages>("language")
            };

            var news = _client?.GetEverything(everythingRequest);

            if (news == null)
                throw new InternalServerErrorHttpException("Could not reach NewsApi");
            if (news.Status != Statuses.Ok)
                throw new BadRequestHttpException(news.Error.Message);

            return news.Articles.Select(article => new NewsApiArticleModel(article));
        }
    }
}