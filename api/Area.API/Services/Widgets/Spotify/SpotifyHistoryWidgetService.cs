using System.Collections.Generic;
using System.Linq;
using Area.API.Exceptions.Http;
using Area.API.Models;
using Area.API.Models.Table.Owned;
using Area.API.Models.Widgets;
using Area.API.Services.Services;
using SpotifyAPI.Web;

namespace Area.API.Services.Widgets.Spotify
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
            SpotifyClient = SpotifyService.ClientFromJson(serviceTokens.Json!);
            return SpotifyClient != null;
        }

        public void CallWidgetApi(WidgetCallParameters widgetCallParams,
            ref WidgetCallResponseModel response)
        {
            var task = SpotifyClient!.Player.GetRecentlyPlayed();
            task.Wait();

            if (!task.IsCompletedSuccessfully)
                throw new InternalServerErrorHttpException("Couldn't reach Spotify");

            response.Items = task.Result.Items?.Select(item => new SpotifyTrackModel(item.Track)) ??
                new List<SpotifyTrackModel>();
        }
    }
}