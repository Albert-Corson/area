using System;
using System.Collections.Generic;
using System.Linq;
using Dashboard.API.Exceptions.Http;
using Dashboard.API.Models;
using Dashboard.API.Models.Table;
using Dashboard.API.Models.Widgets;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NewsAPI;
using NewsAPI.Constants;
using NewsAPI.Models;

namespace Dashboard.API.Services.Widgets.NewsApi
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
        public void CallWidgetApi(HttpContext context, WidgetCallParameters widgetCallParams, ref WidgetCallResponseModel response)
        {
            var everythingRequest = new EverythingRequest {
                From = DateTime.Now.Subtract(TimeSpan.FromDays(31)),
                Q = widgetCallParams.Strings["query"]
            };

            var languageStr = widgetCallParams.Strings["language"];
            if (!string.IsNullOrWhiteSpace(languageStr) && Enum.TryParse<Languages>(languageStr, true, out var language))
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
