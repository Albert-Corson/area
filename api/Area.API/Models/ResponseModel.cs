using Newtonsoft.Json;

namespace Area.API.Models
{
    public class ResponseModel<T> : StatusModel
    {
        [JsonProperty("data", Required = Required.DisallowNull)]
        public T Data { get; set; } = default!;
    }
}