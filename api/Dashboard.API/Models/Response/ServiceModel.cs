using Newtonsoft.Json;

namespace Dashboard.API.Models.Response
{
    public class ServiceModel
    {
        public ServiceModel()
        { }

        public ServiceModel(int id = 0, string name = "")
        {
            Id = id;
            Name = name;
        }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; } = "";
    }
}
