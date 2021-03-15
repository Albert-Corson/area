using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class NewsApiTopHeadlinesWidget : IWidget
    {
        private readonly NewsApiClient _client;

        public NewsApiTopHeadlinesWidget(IConfiguration configuration)
        {
            _client = new NewsApiClient(configuration[AuthConstants.NewsApi.Key]);
        }

        public int Id { get; } = 9;

        public async Task<IEnumerable<WidgetCallResponseItemModel>> CallWidgetApiAsync(
            IEnumerable<ParamModel> widgetCallParams)
        {
            var topHeadlinesRequest = new TopHeadlinesRequest {
                Country = widgetCallParams.GetEnumValue<Countries>("country"),
                Category = widgetCallParams.GetEnumValue<Categories>("category"),
                Language = widgetCallParams.GetEnumValue<Languages>("language")
            };

            var news = await _client.GetTopHeadlinesAsync(topHeadlinesRequest);

            if (news == null)
                throw new InternalServerErrorHttpException("Could not reach NewsApi");
            if (news.Status != Statuses.Ok)
                throw new BadRequestHttpException(news.Error.Message);

            return news.Articles.Select(article => new NewsApiArticleModel(article));
        }
    }
}