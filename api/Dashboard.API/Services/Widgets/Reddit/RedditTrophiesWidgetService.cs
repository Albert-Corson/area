using System.Linq;
using Dashboard.API.Models;
using Dashboard.API.Models.Table.Owned;
using Dashboard.API.Models.Widgets;
using Dashboard.API.Services.Services;
using Microsoft.AspNetCore.Http;
using Reddit;
using RedditSharp.Things;

namespace Dashboard.API.Services.Widgets.Reddit
{
    public class RedditTrophiesWidgetService : IWidgetService
    {
        public RedditTrophiesWidgetService(RedditServiceService redditService)
        {
            RedditService = redditService;
        }

        private RedditServiceService RedditService { get; }

        private RedditClient? RedditClient { get; set; }

        public string Name { get; } = "Reddit trophies";

        public bool ValidateServiceAuth(UserServiceTokensModel serviceTokens)
        {
            RedditClient = RedditService.ClientFromJson(serviceTokens.Json!);
            return RedditClient != null;
        }

        public void CallWidgetApi(HttpContext context, WidgetCallParameters widgetCallParams, ref WidgetCallResponseModel response)
        {
            response.Items = RedditClient!.Account.Trophies().Select(award => new WidgetCallResponseItemModel {
                Header = award.Name,
                Content = award.Description,
                Link = award.URL,
                Image = award.Icon40
            });
        }
    }
}
