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
            services.AddScoped<WidgetManager>();

            services.AddScoped<ImgurGalleryWidget>();
            services.AddScoped<ImgurFavoritesWidget>();
            services.AddScoped<ImgurUploadsWidget>();
            services.AddScoped<ImgurGallerySearchWidget>();
            services.AddScoped<LoremPicsumRandomImageWidget>();
            services.AddScoped<SpotifyFavoriteArtistsWidget>();
            services.AddScoped<SpotifyFavoriteTracksWidget>();
            services.AddScoped<SpotifyHistoryWidget>();
            services.AddScoped<NewsApiTopHeadlinesWidget>();
            services.AddScoped<NewsApiSearchWidget>();
            services.AddScoped<CatApiRandomImagesWidget>();
            services.AddScoped<IcanhazdadjokeRandomJokeWidget>();

            return services;
        }

        public static IServiceCollection AddAreaServices(this IServiceCollection services)
        {
            services.AddScoped<ServiceManager>();

            services.AddScoped<ImgurService>();
            services.AddScoped<SpotifyService>();
            services.AddScoped<MicrosoftService>();

            return services;
        }
    }
}