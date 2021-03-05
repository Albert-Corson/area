using System.Collections.Generic;
using System.Linq;
using Area.API.Exceptions.Http;
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

        public void CallWidgetApi(IEnumerable<ParamModel> _,
            ref WidgetCallResponseModel response)
        {
            var task = SpotifyService.Client!.Player.GetRecentlyPlayed();
            task.Wait();

            if (!task.IsCompletedSuccessfully)
                throw new InternalServerErrorHttpException("Couldn't reach Spotify");

            response.Items = task.Result.Items?.Select(item => new SpotifyTrackModel(item.Track)) ??
                new List<SpotifyTrackModel>();
        }
    }
}