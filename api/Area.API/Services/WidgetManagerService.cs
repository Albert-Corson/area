using System.Collections.Generic;
using System.Linq;
using Area.API.Exceptions.Http;
using Area.API.Models;
using Area.API.Models.Table;
using Area.API.Models.Table.Owned;
using Area.API.Repositories;
using Area.API.Services.Widgets;
using Area.API.Services.Widgets.CatApi;
using Area.API.Services.Widgets.Icanhazdadjoke;
using Area.API.Services.Widgets.Imgur;
using Area.API.Services.Widgets.LoremPicsum;
using Area.API.Services.Widgets.NewsApi;
using Area.API.Services.Widgets.Spotify;
using Area.API.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Area.API.Services
{
    public class WidgetCallParameters
    {
        public IDictionary<string, int?> Integers { get; } = new Dictionary<string, int?>();

        public IDictionary<string, string?> Strings { get; } = new Dictionary<string, string?>();

        public bool TryAddAny(string name, string? value, string type)
        {
            return type switch {
                "string" => Strings.TryAdd(name, value),
                "integer" when value == null => Integers.TryAdd(name, null),
                _ => int.TryParse(value, out var integerValue) && Integers.TryAdd(name, integerValue)
            };
        }

        public bool Contains(string key, string type)
        {
            return type switch {
                "string" => Strings.ContainsKey(key),
                "integer" => Integers.ContainsKey(key),
                _ => false
            };
        }

        public List<WidgetParamModel> MergeAll()
        {
            var list = new List<WidgetParamModel>();

            list.AddRange(Strings.Select(pair => new WidgetParamModel {
                Name = pair.Key,
                Type = "string",
                Value = pair.Value
            }));
            list.AddRange(Integers.Select(pair => new WidgetParamModel {
                Name = pair.Key,
                Type = "integer",
                Value = pair.Value?.ToString()
            }));
            return list;
        }
    }

    public class WidgetManagerService
    {
        private readonly IDictionary<string, IWidgetService> _widgets;
        private readonly UserRepository _userRepository;
        private readonly WidgetRepository _widgetRepository;

        public WidgetManagerService(
            UserRepository userRepository,
            WidgetRepository widgetRepository,
            ImgurGalleryWidgetService imgurGallery,
            ImgurFavoritesWidgetService imgurFavorites,
            ImgurUploadsWidgetService imgurUploads,
            ImgurGallerySearchWidgetService imgurGallerySearch,
            LoremPicsumRandomImageService loremPicsumRandomImage,
            SpotifyFavoriteArtistsWidgetService spotifyFavoriteArtists,
            SpotifyFavoriteTracksWidgetService spotifyFavoriteTracks,
            SpotifyHistoryWidgetService spotifyHistory,
            NewsApiTopHeadlinesWidgetService newsApiTopHeadlines,
            NewsApiSearchWidgetService newsApiSearch,
            CatApiRandomImagesWidgetService catApiRandomImages,
            IcanhazdadjokeRandomJokeWidgetService icanhazdadjokeRandomJoke)
        {
            _userRepository = userRepository;
            _widgetRepository = widgetRepository;
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
                {newsApiSearch.Name, newsApiSearch},
                {catApiRandomImages.Name, catApiRandomImages},
                {icanhazdadjokeRandomJoke.Name, icanhazdadjokeRandomJoke}
            };
        }

        public WidgetCallResponseModel CallWidgetById(HttpContext context, int widgetId)
        {
            var userId = AuthUtilities.GetUserIdFromPrincipal(context.User);

            var user = userId != null ? _userRepository.GetUser(userId) : null;
            if (user == null)
                throw new InternalServerErrorHttpException(); // this means that the JWT falsely validated

            var widget = _widgetRepository.GetWidget(widgetId, true);
            if (widget == null || !_widgets.TryGetValue(widget.Name!, out var widgetService))
                throw new NotFoundHttpException("This widget does not exist");

            if (widget.RequiresAuth == true)
                ValidateSignInState(widgetService, user, widget.ServiceId);

            var widgetCallParams = BuildWidgetCallParams(
                widgetId,
                widget.Params ?? new List<WidgetParamModel>(),
                _userRepository.GetUser(userId, asNoTracking: false)!.WidgetParams!,
                context.Request.Query);

            var response = new WidgetCallResponseModel(widgetCallParams.MergeAll());

            widgetService.CallWidgetApi(context, widgetCallParams, ref response);
            return response;
        }

        public static List<WidgetParamModel> BuildUserWidgetCallParams(IEnumerable<UserWidgetParamModel> userParams, IEnumerable<WidgetParamModel> widgetParams)
        {
            List<WidgetParamModel> parameters = new List<WidgetParamModel>();
            parameters.AddRange(userParams.Select(model => new WidgetParamModel {
                Name = model.Name,
                Required = model.Required,
                Type = model.Type,
                Value = model.Value
            }));
            parameters.AddRange(widgetParams
                .Where(model => !parameters.Exists(param => param.Name == model.Name)));
            return parameters;
        }

        private WidgetCallParameters BuildWidgetCallParams(int widgetId, ICollection<WidgetParamModel> defaultParams, ICollection<UserWidgetParamModel> userParams, IQueryCollection queryParams)
        {
            var callParams = new WidgetCallParameters();

            foreach (var (key, value) in queryParams) {
                string? type = null;
                var userParam = userParams.FirstOrDefault(model => model.WidgetId == widgetId && model.Name == key);
                if (userParam != null) {
                    userParam.Value = GetParamValueByType(value, userParam.Type!);
                    type = userParam.Type!;
                } else {
                    var defaultParam = defaultParams.FirstOrDefault(model => model.Name == key);
                    if (defaultParam != null) {
                        type = defaultParam.Type!;
                        if (defaultParam.Required != true) {
                            userParams.Add(new UserWidgetParamModel {
                                Name = defaultParam.Name,
                                Type = type,
                                Value = GetParamValueByType(value, type!),
                                WidgetId = widgetId
                            });
                        }
                    }
                }

                if (type != null)
                    callParams.TryAddAny(key, value, type);
            }

            foreach (var userParam in userParams) {
                if (userParam.WidgetId != widgetId)
                    continue;
                callParams.TryAddAny(userParam.Name!, userParam.Value!, userParam.Type!);
            }

            foreach (var defaultParam in defaultParams) {
                if (defaultParam.Required != true)
                    callParams.TryAddAny(defaultParam.Name!, defaultParam.Value!, defaultParam.Type!);
                else if (callParams.Contains(defaultParam.Name!, defaultParam.Type!) == false)
                    throw new BadRequestHttpException($"Missing query parameter `{defaultParam.Name}` of type `{defaultParam.Type}`");
            }

            return callParams;
        }

        private static string GetParamValueByType(StringValues value, string paramType)
        {
            if (paramType == "string")
                return value;
            int.TryParse(value, out var integerValue);
            return integerValue.ToString();
        }

        private void ValidateSignInState(IWidgetService widgetService, UserModel user, int serviceId)
        {
            var serviceToken = user.ServiceTokens!.FirstOrDefault(model => model.ServiceId == serviceId);

            if (serviceToken == null)
                throw new UnauthorizedHttpException("You need to sign-in to the service");
            if (widgetService.ValidateServiceAuth(serviceToken))
                return;
            _userRepository.RemoveServiceCredentials(user.Id, serviceId);
            throw new UnauthorizedHttpException("You need to sign-in to the service again");
        }
    }
}
