using System.Collections.Generic;
using System.Net;
using Area.API.Exceptions.Http;
using Area.API.Models;
using Area.API.Models.Table;
using Area.API.Services.Services;
using ICanHazDadJoke.NET;

namespace Area.API.Services.Widgets.Icanhazdadjoke
{
    public class IcanhazdadjokeRandomJokeWidget : IWidget
    {
        public int Id { get; } = 12;

        public void CallWidgetApi(IEnumerable<ParamModel> _,
            ref WidgetCallResponseModel response)
        {
            var client = new DadJokeClient("Area Epitech school project", "https://github.com/Albert-Corson");

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