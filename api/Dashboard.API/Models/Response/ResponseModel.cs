using Newtonsoft.Json;

namespace Dashboard.API.Models.Response
{
    public class ResponseModel<T> : StatusModel
    {
        [JsonProperty("data")]
        public T Data { get; set; } = default!;
    }
}
