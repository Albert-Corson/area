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
    public class NewsApiTopHeadlinesWidgetService : IWidgetService
    {
        private readonly NewsApiClient? _client;

        public NewsApiTopHeadlinesWidgetService(IConfiguration configuration)
        {
            var apiKey = configuration[AuthConstants.NewsApi.Key];

            if (apiKey != null)
                _client = new NewsApiClient(apiKey);
        }

        public string Name { get; } = "Top headlines";

        public void CallWidgetApi(IEnumerable<ParamModel> widgetCallParams,
            ref WidgetCallResponseModel response)
        {
            var countryStr = widgetCallParams.GetValue("country");
            var categoryStr = widgetCallParams.GetValue("category");
            var languageStr = widgetCallParams.GetValue("language");

            var topHeadlinesRequest = new TopHeadlinesRequest();

            if (!string.IsNullOrWhiteSpace(countryStr) && Enum.TryParse<Countries>(countryStr, true, out var country))
                topHeadlinesRequest.Country = country;

            if (!string.IsNullOrWhiteSpace(categoryStr) &&
                Enum.TryParse<Categories>(categoryStr, true, out var category))
                topHeadlinesRequest.Category = category;

            if (!string.IsNullOrWhiteSpace(languageStr) &&
                Enum.TryParse<Languages>(languageStr, true, out var language))
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