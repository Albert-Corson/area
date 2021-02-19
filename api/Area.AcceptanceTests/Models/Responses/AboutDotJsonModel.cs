using System.Collections.Generic;
using Newtonsoft.Json;

namespace Area.AcceptanceTests.Models.Responses
{
    public class AboutDotJsonModel
    {
        [JsonProperty("services", Required = Required.Always)]
        public IEnumerable<ServiceModel> Services { get; set; } = null!;
    }
}