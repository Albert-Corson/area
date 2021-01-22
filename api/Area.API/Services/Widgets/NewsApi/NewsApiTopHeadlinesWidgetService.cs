using System;
using System.Linq;
using Area.API.Exceptions.Http;
using Area.API.Models;
using Area.API.Models.Widgets;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using NewsAPI;
using NewsAPI.Constants;
using NewsAPI.Models;

namespace Area.API.Services.Widgets.NewsApi
{
    public class NewsApiTopHeadlinesWidgetService : IWidgetService
    {
        private readonly NewsApiClient? _client;

        public NewsApiTopHeadlinesWidgetService(IConfiguration configuration)
        {
            var newsApiConf = configuration.GetSection("WidgetApiKeys").GetSection("NewsApi");
            var apiKey = newsApiConf?["ApiKey"];

            if (apiKey != null)
                _client = new NewsApiClient(apiKey);
        }

        public string Name { get; } = "Top headlines";

        public void CallWidgetApi(HttpContext context, WidgetCallParameters widgetCallParams, ref WidgetCallResponseModel response)
        {
            var countryStr = widgetCallParams.Strings["country"];
            var categoryStr = widgetCallParams.Strings["category"];
            var languageStr = widgetCallParams.Strings["language"];

            var topHeadlinesRequest = new TopHeadlinesRequest();

            if (!string.IsNullOrWhiteSpace(countryStr) && Enum.TryParse<Countries>(countryStr, true, out var country))
                topHeadlinesRequest.Country = country;

            if (!string.IsNullOrWhiteSpace(categoryStr) && Enum.TryParse<Categories>(categoryStr, true, out var category))
                topHeadlinesRequest.Category = category;

            if (!string.IsNullOrWhiteSpace(languageStr) && Enum.TryParse<Languages>(languageStr, true, out var language))
                topHeadlinesRequest.Language = language;
            else
                topHeadlinesRequest.Language = Languages.EN;

            var news = _client?.GetTopHeadlines(topHeadlinesRequest);

            if (news == null)
                throw new InternalServerErrorHttpException("Could not reach NewsApi");
            if (news.Status != Statuses.Ok)
                throw new BadRequestHttpException(news.Error.Message);

            response.Items = news.Articles.Select(article => new NewsApiArticleModel(article));
        }
    }
}
