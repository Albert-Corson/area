using System;
using System.Collections.Generic;
using System.Linq;
using Area.API.Constants;
using Area.API.Exceptions.Http;
using Area.API.Extensions;
using Area.API.Models;
using Area.API.Models.Table;
using Area.API.Models.Widgets;
using Microsoft.Extensions.Configuration;
using NewsAPI;
using NewsAPI.Constants;
using NewsAPI.Models;

namespace Area.API.Services.Widgets.NewsApi
{
    public class NewsApiSearchWidgetService : IWidgetService
    {
        private readonly NewsApiClient? _client;

        public NewsApiSearchWidgetService(IConfiguration configuration)
        {
            var apiKey = configuration[AuthConstants.NewsApi.Key];

            if (apiKey != null)
                _client = new NewsApiClient(apiKey);
        }

        public string Name { get; } = "News search";

        public void CallWidgetApi(IEnumerable<ParamModel> widgetCallParams,
            ref WidgetCallResponseModel response)
        {
            var everythingRequest = new EverythingRequest {
                From = DateTime.Now.Subtract(TimeSpan.FromDays(31)),
                Q = widgetCallParams.GetValue("query")
            };

            var languageStr = widgetCallParams.GetValue("language");
            if (!string.IsNullOrWhiteSpace(languageStr) &&
                Enum.TryParse<Languages>(languageStr, true, out var language))
                everythingRequest.Language = language;

            var news = _client?.GetEverything(everythingRequest);

            if (news == null)
                throw new InternalServerErrorHttpException("Could not reach NewsApi");
            if (news.Status != Statuses.Ok)
                throw new BadRequestHttpException(news.Error.Message);

            response.Items = news.Articles.Select(article => new NewsApiArticleModel(article));
        }
    }
}