using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Dashboard.API.Models.Table.ManyToMany;
using Newtonsoft.Json;

namespace Dashboard.API.Models.Table
{
    public class ServiceModel
    {
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
