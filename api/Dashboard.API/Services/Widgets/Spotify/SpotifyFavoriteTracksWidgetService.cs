using System;
using System.Collections.Generic;
using Dashboard.API.Exceptions.Http;
using Dashboard.API.Models;
using Dashboard.API.Models.Table;
using Dashboard.API.Models.Table.Owned;
using Dashboard.API.Services.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpotifyAPI.Web;

namespace Dashboard.API.Services.Widgets.Spotify
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
            SpotifyClient = SpotifyService.CreateClientFromJsonTokens(serviceTokens.Json!);
            return SpotifyClient != null;
        }

        public JsonResult CallWidgetApi(HttpContext context, UserModel user, WidgetModel widget, WidgetCallParameters widgetCallParams)
        {
            var timeRangeStr = widgetCallParams.Strings["time_range"];

            var timeRange = timeRangeStr switch {
                "long_term" => PersonalizationTopRequest.TimeRange.LongTerm,
                "medium_term" => PersonalizationTopRequest.TimeRange.MediumTerm,
                "short_term" => PersonalizationTopRequest.TimeRange.ShortTerm,
                _ => throw new BadRequestHttpException($"Query parameter `time_range` has an invalid value `{timeRangeStr}`. Expected long_term|medium_term|short_term")
            };

            var task = SpotifyClient!.Personalization.GetTopTracks(new PersonalizationTopRequest {
                TimeRangeParam = timeRange
            });
            task.Wait();

            if (!task.IsCompletedSuccessfully)
                throw new InternalServerErrorHttpException("Couldn't reach Spotify");

            // TODO: transpose the received data to intermediate class
            return new ResponseModel<List<FullTrack>> {
                Data = task.Result.Items ?? new List<FullTrack>()
            };
        }
    }
}
