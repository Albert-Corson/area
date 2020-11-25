using Newtonsoft.Json;

namespace Dashboard.API.Models.Response
{
    public class ResponseModel<T> : StatusModel where T : new()
    {
        public ResponseModel()
        { }

        public ResponseModel(bool successful, string? error = null)
            : base(successful, error)
        { }

        public ResponseModel(bool successful, T data, string? error = null)
            : base(successful, error)
        {
            Data = data;
        }

        [JsonProperty("data")]
        public T Data { get; set; } = new T();
    }
}
