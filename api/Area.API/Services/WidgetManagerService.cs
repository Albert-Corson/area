using System.Collections.Generic;
using System.Linq;
using Area.API.Exceptions.Http;
using Area.API.Extensions;
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
using Microsoft.AspNetCore.Http;

namespace Area.API.Services
{
    public class WidgetManagerService
    {
        private readonly UserRepository _userRepository;
        private readonly WidgetRepository _widgetRepository;
        private readonly IDictionary<string, IWidgetService> _widgets;

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
            if (!context.User.TryGetUserId(out var userId))
                throw new InternalServerErrorHttpException(); // this means that the JWT falsely validated

            var user = _userRepository.GetUser(userId, includeChildren: true, asNoTracking: false);
            if (user == null)
                throw new InternalServerErrorHttpException();

            var widget = _widgetRepository.GetWidget(widgetId, true);
            if (widget == null || !_widgets.TryGetValue(widget.Name!, out var widgetService))
                throw new NotFoundHttpException("This widget does not exist");

            if (widget.RequiresAuth)
                ValidateSignInState(widgetService, user, widget.ServiceId);

            var widgetCallParams = BuildWidgetCallParams(
                widgetId,
                widget.Params,
                user.WidgetParams,
                context.Request.Query);

            var response = new WidgetCallResponseModel(widgetCallParams);

            widgetService.CallWidgetApi(widgetCallParams, ref response);
            return response;
        }

        public static List<ParamModel> BuildUserWidgetCallParams(IEnumerable<UserParamModel> userParams,
            IEnumerable<ParamModel> widgetParams)
        {
            List<ParamModel> parameters = new List<ParamModel>();
            parameters.AddRange(userParams.Select(model => new ParamModel {
                Name = model.Param.Name,
                Required = model.Param.Required,
                Type = model.Param.Type,
                Value = model.Value
            }));
            parameters.AddRange(widgetParams
                .Where(model => !parameters.Exists(param => param.Name == model.Name)));
            return parameters;
        }

        private static List<ParamModel> BuildWidgetCallParams(int widgetId, ICollection<ParamModel> defaultParams,
            ICollection<UserParamModel> userParams, IQueryCollection queryParams)
        {
            List<ParamModel> callParams = new List<ParamModel>();
            
            foreach (var (key, value) in queryParams) {
                var userParam = userParams.FirstOrDefault(model => model.Param.WidgetId == widgetId && model.Param.Name == key);
                if (userParam != null) {
                    userParam.Value = value;
                } else {
                    var defaultParam = defaultParams.FirstOrDefault(model => model.Name == key);
                    if (defaultParam?.Required == false) {
                        userParams.Add(new UserParamModel {
                            Value = value,
                            ParamId = defaultParam.Id,
                            Param = defaultParam
                        });
                    } else if (defaultParam?.Required == true) {
                        callParams.Add(defaultParam);
                    }
                }
            }

            callParams.AddRange(from userParam in userParams
                where userParam.Param == null || userParam.Param.WidgetId == widgetId
                select new ParamModel(userParam));

            foreach (var defaultParam in defaultParams) {
                var contains = callParams.FirstOrDefault(model => model.Id == defaultParam.Id) != null;
                if (defaultParam.Required != true && !contains)
                    callParams.Add(defaultParam);
                else if (!contains)
                    throw new BadRequestHttpException(
                        $"Missing query parameter `{defaultParam.Name}` of type `{defaultParam.Type}`");
            }

            return callParams;
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