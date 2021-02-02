using System;
using System.Linq;
using Area.API.Exceptions.Http;
using Area.API.Models;
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
            var newsApiConf = configuration.GetSection("WidgetApiKeys").GetSection("NewsApi");
            var apiKey = newsApiConf?["ApiKey"];

            if (apiKey != null)
                _client = new NewsApiClient(apiKey);
        }

        public string Name { get; } = "News search";

        public void CallWidgetApi(WidgetCallParameters widgetCallParams,
            ref WidgetCallResponseModel response)
        {
            var everythingRequest = new EverythingRequest {
                From = DateTime.Now.Subtract(TimeSpan.FromDays(31)),
                Q = widgetCallParams.Strings["query"]
            };

            var languageStr = widgetCallParams.Strings["language"];
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