using System.Collections.Generic;
using Newtonsoft.Json;

namespace Area.AcceptanceTests.Models.Responses
{
    public class ServiceModel
    {
        public void Copy(ServiceModel other)
        {
            Id = other.Id;
            Name = other.Name;
        }
        
        public static bool operator!=(ServiceModel self, ServiceModel other)
        {
            return !(self == other);
        }

        public static bool operator==(ServiceModel self, ServiceModel other)
        {
            return self.Id == other.Id
                && self.Name == other.Name;
        }

        [JsonProperty("id", Required = Required.Always)]
        public int Id { get; set; }

        [JsonProperty("name", Required = Required.Always)]
        public string Name { get; set; } = null!;
    }
}