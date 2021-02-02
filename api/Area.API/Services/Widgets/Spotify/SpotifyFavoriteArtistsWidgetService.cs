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
    public class SpotifyFavoriteArtistsWidgetService : IWidgetService
    {
        public SpotifyFavoriteArtistsWidgetService(SpotifyServiceService spotify)
        {
            SpotifyService = spotify;
        }

        private SpotifyServiceService SpotifyService { get; }

        private SpotifyClient? SpotifyClient { get; set; }

        public string Name { get; } = "Spotify favorite artists";

        public bool ValidateServiceAuth(UserServiceTokensModel serviceTokens)
        {
            SpotifyClient = SpotifyService.ClientFromJson(serviceTokens.Json!);
            return SpotifyClient != null;
        }

        public void CallWidgetApi(WidgetCallParameters widgetCallParams,
            ref WidgetCallResponseModel response)
        {
            var timeRangeStr = widgetCallParams.Strings["time_range"];

            var timeRange = timeRangeStr switch {
                "long_term" => PersonalizationTopRequest.TimeRange.LongTerm,
                "medium_term" => PersonalizationTopRequest.TimeRange.MediumTerm,
                "short_term" => PersonalizationTopRequest.TimeRange.ShortTerm,
                _ => throw new BadRequestHttpException(
                    $"Query parameter `time_range` has an invalid value `{timeRangeStr}`. Expected long_term|medium_term|short_term")
            };

            var task = SpotifyClient!.Personalization.GetTopArtists(new PersonalizationTopRequest {
                TimeRangeParam = timeRange
            });
            task.Wait();

            if (!task.IsCompletedSuccessfully)
                throw new InternalServerErrorHttpException("Couldn't reach Spotify");

            // TODO: transpose the received data to intermediate class
            response.Items = task.Result.Items?.Select(artist => new SpotifyArtistModel(artist)) ??
                new List<SpotifyArtistModel>();
        }
    }
}