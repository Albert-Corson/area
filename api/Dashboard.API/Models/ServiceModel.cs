using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Dashboard.API.Models
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

        [ForeignKey("ServiceId")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonProperty("id")]
        public int? Id { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonIgnore]
        public ICollection<WidgetModel>? Widgets { get; set; }

        [JsonIgnore]
        public ICollection<UserServiceModel>? Users { get; set; }
    }
}
