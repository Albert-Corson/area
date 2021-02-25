using Area.API.Repositories;
using Area.API.Services;
using Area.API.Services.Services;
using Area.API.Services.Widgets.CatApi;
using Area.API.Services.Widgets.Icanhazdadjoke;
using Area.API.Services.Widgets.Imgur;
using Area.API.Services.Widgets.LoremPicsum;
using Area.API.Services.Widgets.NewsApi;
using Area.API.Services.Widgets.Spotify;
using Microsoft.Extensions.DependencyInjection;

namespace Area.API.Installers
{
    public static class AreaInstaller
    {
        public static IServiceCollection AddAreaRepositories(this IServiceCollection services)
        {
            services.AddTransient<UserRepository>();
            services.AddTransient<ServiceRepository>();
            services.AddTransient<WidgetRepository>();

            return services;
        }

        public static IServiceCollection AddAreaWidgets(this IServiceCollection services)
        {
            services.AddScoped<WidgetManagerService>();

            services.AddScoped<ImgurGalleryWidgetService>();
            services.AddScoped<ImgurFavoritesWidgetService>();
            services.AddScoped<ImgurUploadsWidgetService>();
            services.AddScoped<ImgurGallerySearchWidgetService>();
            services.AddScoped<LoremPicsumRandomImageService>();
            services.AddScoped<SpotifyFavoriteArtistsWidgetService>();
            services.AddScoped<SpotifyFavoriteTracksWidgetService>();
            services.AddScoped<SpotifyHistoryWidgetService>();
            services.AddScoped<NewsApiTopHeadlinesWidgetService>();
            services.AddScoped<NewsApiSearchWidgetService>();
            services.AddScoped<CatApiRandomImagesWidgetService>();
            services.AddScoped<IcanhazdadjokeRandomJokeWidgetService>();

            return services;
        }

        public static IServiceCollection AddAreaServices(this IServiceCollection services)
        {
            services.AddScoped<ServiceManagerService>();

            services.AddScoped<ImgurServiceService>();
            services.AddScoped<SpotifyServiceService>();

            return services;
        }
    }
}