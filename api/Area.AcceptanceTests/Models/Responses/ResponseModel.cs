using Newtonsoft.Json;

namespace Area.AcceptanceTests.Models.Responses
{
    public class ResponseModel<TData> : StatusModel where TData : class
    {
        [JsonProperty("data", Required = Required.DisallowNull)]
        public TData? Data { get; set; }
    }
}