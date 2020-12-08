using System.Collections.Generic;
using System.Linq;
using Dashboard.API.Exceptions.Http;
using Dashboard.API.Models;
using Dashboard.API.Models.Table.Owned;
using Dashboard.API.Models.Widgets;
using Dashboard.API.Services.Services;
using Microsoft.AspNetCore.Http;
using SpotifyAPI.Web;

namespace Dashboard.API.Services.Widgets.Spotify
{
    public class SpotifyHistoryWidgetService : IWidgetService
    {
        public SpotifyHistoryWidgetService(SpotifyServiceService spotify)
        {
            SpotifyService = spotify;
        }

        private SpotifyServiceService SpotifyService { get; }

        private SpotifyClient? SpotifyClient { get; set; }

        public string Name { get; } = "Spotify history";

        public bool ValidateServiceAuth(UserServiceTokensModel serviceTokens)
        {
            SpotifyClient = SpotifyService.CreateClientFromJsonTokens(serviceTokens.Json!);
            return SpotifyClient != null;
        }

        public void CallWidgetApi(HttpContext context, WidgetCallParameters widgetCallParams, ref WidgetCallResponseModel response)
        {

            var task = SpotifyClient!.Player.GetRecentlyPlayed();
            task.Wait();

            if (!task.IsCompletedSuccessfully)
                throw new InternalServerErrorHttpException("Couldn't reach Spotify");

            response.Items = task.Result.Items?.Select(item => new SpotifyTrackModel(item.Track)) ?? new List<SpotifyTrackModel>();
        }
    }
}
