using System.Collections.Generic;
using System.Linq;
using Area.API.Exceptions.Http;
using Area.API.Extensions;
using Area.API.Models;
using Area.API.Models.Table;
using Area.API.Models.Table.Owned;
using Area.API.Models.Widgets;
using Area.API.Services.Services;
using SpotifyAPI.Web;

namespace Area.API.Services.Widgets.Spotify
{
    public class SpotifyFavoriteTracksWidgetService : IWidgetService
    {
        public SpotifyFavoriteTracksWidgetService(SpotifyServiceService spotify)
        {
            SpotifyService = spotify;
        }

        private SpotifyServiceService SpotifyService { get; }

        private SpotifyClient? SpotifyClient { get; set; }

        public string Name { get; } = "Spotify favorite tracks";

        public bool ValidateServiceAuth(UserServiceTokensModel serviceTokens)
        {
            SpotifyClient = SpotifyService.ClientFromJson(serviceTokens.Json!);
            return SpotifyClient != null;
        }

        public void CallWidgetApi(IEnumerable<ParamModel> widgetCallParams,
            ref WidgetCallResponseModel response)
        {
            var timeRange = widgetCallParams.GetEnumValue<PersonalizationTopRequest.TimeRange>("time_range");

            var task = SpotifyClient!.Personalization.GetTopTracks(new PersonalizationTopRequest {
                TimeRangeParam = timeRange
            });
            task.Wait();

            if (!task.IsCompletedSuccessfully)
                throw new InternalServerErrorHttpException("Couldn't reach Spotify");

            response.Items = task.Result.Items?.Select(track => new SpotifyTrackModel(track)) ??
                new List<SpotifyTrackModel>();
        }
    }
}