using System;
using System.Collections.Generic;
using Dashboard.API.Exceptions.Http;
using Dashboard.API.Models;
using Dashboard.API.Models.Table;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NewsAPI;
using NewsAPI.Constants;
using NewsAPI.Models;

namespace Dashboard.API.Services.Widgets.NewsApi
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
        public JsonResult CallWidgetApi(HttpContext context, UserModel user, WidgetModel widget, WidgetCallParameters widgetCallParams)
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

            return new ResponseModel<List<Article>> {
                Data = news.Articles
            };
        }
    }
}
