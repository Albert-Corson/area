using Newtonsoft.Json;

namespace Area.AcceptanceTests.Models.Responses
{
    public class ResponseModel<T> : StatusModel where T : class
    {
        [JsonProperty("data", Required = Required.DisallowNull)]
        public T? Data { get; set; }
    }
}