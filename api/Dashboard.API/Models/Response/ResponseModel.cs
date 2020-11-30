using Newtonsoft.Json;

namespace Dashboard.API.Models.Response
{
    public class ResponseModel<T> : StatusModel where T: class, new()
    {
        [JsonProperty("data")]
        public T Data { get; set; } = new T();
    }
}
