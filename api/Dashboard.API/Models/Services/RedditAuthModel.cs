using Newtonsoft.Json;

namespace Dashboard.API.Models.Services
{
    public class RedditAuthModel
    {
        [Swan.Formatters.JsonProperty("access_token")]
        public string? AccessToken { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.None, new JsonSerializerSettings {
                NullValueHandling = NullValueHandling.Ignore
            });
        }
    }
}
