using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace Area.API.Models.Table
{
    [Table("Services")]
    public class ServiceModel
    {
        [ForeignKey("ServiceId")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonProperty("id", Required = Required.Always)]
        [SwaggerSchema("Service's ID")]
        public int Id { get; set; }

        [JsonProperty("name", Required = Required.Always)]
        [SwaggerSchema("Service's name")]
        public string Name { get; set; } = null!;

        [JsonIgnore]
        public ICollection<WidgetModel> Widgets { get; set; } = null!;
    }
}