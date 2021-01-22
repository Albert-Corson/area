using Newtonsoft.Json;

namespace Area.API.Models
{
    public class ResponseModel<T> : StatusModel
    {
        [JsonProperty("data")]
        public T Data { get; set; } = default!;
    }
}
