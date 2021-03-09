using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Area.API.Exceptions.Http;
using Area.API.Models;
using Area.API.Models.Table;
using ICanHazDadJoke.NET;

namespace Area.API.Services.Widgets.Icanhazdadjoke
{
    public class IcanhazdadjokeRandomJokeWidget : IWidget
    {
        public int Id { get; } = 12;

        public async Task<IEnumerable<WidgetCallResponseItemModel>> CallWidgetApiAsync(IEnumerable<ParamModel> _)
        {
            var client = new DadJokeClient("Area Epitech school project", "https://github.com/Albert-Corson");

            var result = await client.GetRandomJokeAsync();

            if (result.Status != (int) HttpStatusCode.OK)
                throw new InternalServerErrorHttpException("Could not reach icanhazdadjoke");

            return new[] {
                new WidgetCallResponseItemModel {
                    Content = result.Joke
                }
            };
        }
    }
}