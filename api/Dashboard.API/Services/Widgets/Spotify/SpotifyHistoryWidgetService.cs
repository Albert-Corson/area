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

        public JsonResult CallWidgetApi(HttpContext context, UserModel user, WidgetModel widget, WidgetCallParameters widgetCallParams)
        {

            var task = SpotifyClient!.Player.GetRecentlyPlayed();
            task.Wait();

            if (!task.IsCompletedSuccessfully)
                throw new InternalServerErrorHttpException("Couldn't reach Spotify");

            // TODO: transpose the received data to intermediate class
            return new ResponseModel<List<PlayHistoryItem>> {
                Data = task.Result.Items ?? new List<PlayHistoryItem>()
            };
        }
    }
}
