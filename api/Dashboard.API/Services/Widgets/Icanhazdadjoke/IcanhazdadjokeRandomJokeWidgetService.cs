using System.Net;
using Dashboard.API.Exceptions.Http;
using Dashboard.API.Models;
using ICanHazDadJoke.NET;
using Microsoft.AspNetCore.Http;

namespace Dashboard.API.Services.Widgets.Icanhazdadjoke
{
    public class IcanhazdadjokeRandomJokeWidgetService : IWidgetService
    {
        public string Name { get; } = "Random dad joke";

        public void CallWidgetApi(HttpContext context, WidgetCallParameters widgetCallParams, ref WidgetCallResponseModel response)
        {
            var client = new DadJokeClient("Dashboard Epitech school project", "https://github.com/Albert-Corson");

            var task = client.GetRandomJokeAsync();
            task.Wait();

            if (!task.IsCompletedSuccessfully || task.Result.Status != (int) HttpStatusCode.OK)
                throw new InternalServerErrorHttpException("Could not reach icanhazdadjoke");

            response.Item = new WidgetCallResponseItemModel {
                Content = task.Result.Joke
            };
        }
    }
}
