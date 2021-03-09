using System.Collections.Generic;
using System.Linq;
using Area.API.Exceptions.Http;
using Area.API.Extensions;
using Area.API.Models;
using Area.API.Models.Table;
using Area.API.Models.Widgets;
using Area.API.Services.Services;
using SpotifyAPI.Web;

namespace Area.API.Services.Widgets.Spotify
{
    public class SpotifyFavoriteTracksWidget : IWidget
    {
        public SpotifyFavoriteTracksWidget(SpotifyService spotify)
        {
            SpotifyService = spotify;
        }

        private SpotifyService SpotifyService { get; }

        public int Id { get; } = 7;

        public IEnumerable<WidgetCallResponseItemModel> CallWidgetApi(IEnumerable<ParamModel> widgetCallParams)
        {
            var timeRange = widgetCallParams.GetEnumValue<PersonalizationTopRequest.TimeRange>("time_range");

            var task = SpotifyService.Client!.Personalization.GetTopTracks(new PersonalizationTopRequest {
                TimeRangeParam = timeRange
            });
            task.Wait();

            if (!task.IsCompletedSuccessfully)
                throw new InternalServerErrorHttpException("Couldn't reach Spotify");

            return task.Result.Items?.Select(track => new SpotifyTrackModel(track)) ??
                new List<SpotifyTrackModel>();
        }
    }
}