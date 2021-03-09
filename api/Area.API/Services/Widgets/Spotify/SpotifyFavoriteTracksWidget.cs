using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<IEnumerable<WidgetCallResponseItemModel>> CallWidgetApiAsync(
            IEnumerable<ParamModel> widgetCallParams)
        {
            var timeRange = widgetCallParams.GetEnumValue<PersonalizationTopRequest.TimeRange>("time_range");

            var result = await SpotifyService.Client!.Personalization
                .GetTopTracks(new PersonalizationTopRequest {
                TimeRangeParam = timeRange
            });

            return result.Items?.Select(track => new SpotifyTrackModel(track)) ??
                new List<SpotifyTrackModel>();
        }
    }
}