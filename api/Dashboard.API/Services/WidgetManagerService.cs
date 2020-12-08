using System.Collections.Generic;
using System.Linq;
using Dashboard.API.Exceptions.Http;
using Dashboard.API.Models.Table.Owned;
using Dashboard.API.Repositories;
using Dashboard.API.Services.Widgets;
using Dashboard.API.Services.Widgets.Imgur;
using Dashboard.API.Services.Widgets.LoremPicsum;
using Dashboard.API.Services.Widgets.NewsApi;
using Dashboard.API.Services.Widgets.Spotify;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;

namespace Dashboard.API.Services
{
    public class WidgetCallParameters
    {
        public IDictionary<string, int?> Integers { get; } = new Dictionary<string, int?>();

        public IDictionary<string, string?> Strings { get; } = new Dictionary<string, string?>();

        public IDictionary<string, string?> Undefined { get; } = new Dictionary<string, string?>();

        public bool TryAddAny(string name, string? value, string type = "")
        {
            return type.ToLower() switch {
                "string" => Strings.TryAdd(name, value),
                "integer" when value == null => Integers.TryAdd(name, null),
                "integer" => int.TryParse(value, out var integerValue) && Integers.TryAdd(name, integerValue),
                _ => Undefined.TryAdd(name, value)
            };
        }
    }

    public class WidgetManagerService
    {
        private readonly DatabaseRepository _database;
        private readonly IDictionary<string, IWidgetService> _widgets;

        public WidgetManagerService(
            DatabaseRepository database,
            ImgurGalleryWidgetService imgurGallery,
            ImgurFavoritesWidgetService imgurFavorites,
            ImgurUploadsWidgetService imgurUploads,
            ImgurGallerySearchWidgetService imgurGallerySearch,
            LoremPicsumRandomImageService loremPicsumRandomImage,
            SpotifyFavoriteArtistsWidgetService spotifyFavoriteArtists,
            SpotifyFavoriteTracksWidgetService spotifyFavoriteTracks,
            SpotifyHistoryWidgetService spotifyHistory,
            NewsApiTopHeadlinesWidgetService newsApiTopHeadlines,
            NewsApiSearchWidgetService newsApiSearch)
        {
            _database = database;
            _widgets = new Dictionary<string, IWidgetService> {
                {imgurGallery.Name, imgurGallery},
                {imgurFavorites.Name, imgurFavorites},
                {imgurUploads.Name, imgurUploads},
                {loremPicsumRandomImage.Name, loremPicsumRandomImage},
                {imgurGallerySearch.Name, imgurGallerySearch},
                {spotifyFavoriteArtists.Name, spotifyFavoriteArtists},
                {spotifyFavoriteTracks.Name, spotifyFavoriteTracks},
                {spotifyHistory.Name, spotifyHistory},
                {newsApiTopHeadlines.Name, newsApiTopHeadlines},
                {newsApiSearch.Name, newsApiSearch}
            };
        }

        public JsonResult CallWidgetById(HttpContext context, int widgetId)
        {
            var userId = AuthService.GetUserIdFromPrincipal(context.User);
            if (userId == null)
                throw new UnauthorizedHttpException();

            var user = _database.Users
                .Include(model => model.WidgetParams!
                    .Where(paramModel => paramModel.WidgetId == widgetId))
                .FirstOrDefault(model => model.Id == userId);
            if (user == null)
                throw new UnauthorizedHttpException();

            var widget = _database.Widgets
                .Include(model => model.DefaultParams)
                .FirstOrDefault(model => model.Id == widgetId);
            if (widget == null || !_widgets.TryGetValue(widget.Name!, out var widgetService))
                throw new NotFoundHttpException("Widget not found");

            if (widget.RequiresAuth == true)
                ValidateSignInState(widgetService, widget.ServiceId!.Value);

            var widgetCallParams = BuildWidgetCallParams(
                widgetId,
                widget.DefaultParams ?? new List<WidgetParamModel>(),
                user.WidgetParams ?? new List<UserWidgetParamModel>(),
                context.Request.Query);

            return widgetService.CallWidgetApi(context, user, widget, widgetCallParams);
        }

        private WidgetCallParameters BuildWidgetCallParams(int widgetId, ICollection<WidgetParamModel> defaultParams, ICollection<UserWidgetParamModel> userParams, IQueryCollection queryParams)
        {
            UpdateUserParamsWithQueryParams(widgetId, defaultParams, userParams, queryParams);

            var callParams = new WidgetCallParameters();

            foreach (var (key, value) in queryParams) {
                var type = "";
                var userParam = userParams.FirstOrDefault(model => model.Name == key);
                if (userParam != null) {
                    userParam.Value = GetParamValueByType(value, userParam.Type!);
                    type = userParam.Type!;
                } else {
                    var defaultParam = defaultParams.FirstOrDefault(model => model.Name == key);
                    if (defaultParam != null) {
                        userParams.Add(new UserWidgetParamModel {
                            Name = defaultParam.Name,
                            Type = defaultParam.Type,
                            WidgetId = widgetId,
                            Value = GetParamValueByType(value, defaultParam.Type!)
                        });
                        type = defaultParam.Type!;
                    }
                }

                callParams.TryAddAny(key, value, type);
            }

            _database.SaveChanges();

            foreach (var userParam in userParams) {
                callParams.TryAddAny(userParam.Name!, userParam.Value!, userParam.Type!);
            }

            foreach (var defaultParam in defaultParams) {
                callParams.TryAddAny(defaultParam.Name!, defaultParam.Value!, defaultParam.Type!);
            }

            return callParams;
        }

        private void UpdateUserParamsWithQueryParams(int widgetId, ICollection<WidgetParamModel> defaultParams, ICollection<UserWidgetParamModel> userParams, IQueryCollection queryParams)
        {
            foreach (var (key, value) in queryParams) {
                var userParam = userParams.FirstOrDefault(model => model.Name == key);
                if (userParam != null) {
                    userParam.Value = GetParamValueByType(value, userParam.Type!);
                } else {
                    var defaultParam = defaultParams.FirstOrDefault(model => model.Name == key);
                    if (defaultParam == null)
                        continue;
                    userParams.Add(new UserWidgetParamModel {
                        Name = defaultParam.Name,
                        Type = defaultParam.Type,
                        WidgetId = widgetId,
                        Value = GetParamValueByType(value, defaultParam.Type!)
                    });
                }
            }

            _database.SaveChanges();
        }

        private static string GetParamValueByType(StringValues value, string paramType)
        {
            if (paramType == "string")
                return value;
            int.TryParse(value, out var integerValue);
            return integerValue.ToString();
        }

        private void ValidateSignInState(IWidgetService widgetService, int serviceId)
        {
            var serviceTokens = _database.Users
                .AsNoTracking()
                .SelectMany(model => model.ServiceTokens)
                .FirstOrDefault(tokensModel => tokensModel.ServiceId == serviceId);
            if (serviceTokens == null)
                throw new UnauthorizedHttpException("You need to be signed-in to the service");
            if (widgetService.ValidateServiceAuth(serviceTokens))
                return;
            _database.Remove(serviceTokens);
            throw new UnauthorizedHttpException("You need to be sign-in again to the service");
        }
    }
}
