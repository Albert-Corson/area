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
    public class NewsApiTopHeadlinesWidget : IWidget
    {
        private readonly NewsApiClient? _client;

        public NewsApiTopHeadlinesWidget(IConfiguration configuration)
        {
            var apiKey = configuration[AuthConstants.NewsApi.Key];

            if (apiKey != null)
                _client = new NewsApiClient(apiKey);
        }

        public int Id { get; } = 9;

        public void CallWidgetApi(IEnumerable<ParamModel> widgetCallParams,
            ref WidgetCallResponseModel response)
        {

            var topHeadlinesRequest = new TopHeadlinesRequest {
                Country = widgetCallParams.GetEnumValue<Countries>("country"),
                Category = widgetCallParams.GetEnumValue<Categories>("category"),
                Language = widgetCallParams.GetEnumValue<Languages>("language")
            };

            var news = _client?.GetTopHeadlines(topHeadlinesRequest);

            if (news == null)
                throw new InternalServerErrorHttpException("Could not reach NewsApi");
            if (news.Status != Statuses.Ok)
                throw new BadRequestHttpException(news.Error.Message);

            response.Items = news.Articles.Select(article => new NewsApiArticleModel(article));
        }
    }
}