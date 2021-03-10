using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Area.API.Models;
using Area.API.Models.Table;
using Area.API.Models.Widgets;
using Area.API.Services.Services;

namespace Area.API.Services.Widgets.Spotify
{
    public class SpotifyHistoryWidget : IWidget
    {
        public SpotifyHistoryWidget(SpotifyService spotify)
        {
            SpotifyService = spotify;
        }

        private SpotifyService SpotifyService { get; }

        public int Id { get; } = 8;

        public async Task<IEnumerable<WidgetCallResponseItemModel>> CallWidgetApiAsync(IEnumerable<ParamModel> _)
        {
            var result = await SpotifyService.Client!.Player.GetRecentlyPlayed();

            return result.Items?.Select(item => new SpotifyTrackModel(item.Track)) ??
                new List<SpotifyTrackModel>();
        }
    }
}