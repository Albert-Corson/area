using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Area.API.Models.Table.ManyToMany;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace Area.API.Models.Table
{
    [Table("Widgets")]
    public class WidgetModel
    {
        [ForeignKey("WidgetId")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonProperty("id", Required = Required.Always)]
        [SwaggerSchema("Widget's ID")]
        public int Id { get; set; }

        [JsonProperty("name", Required = Required.Always)]
        [SwaggerSchema("Widget's name")]
        public string Name { get; set; } = null!;

        [JsonProperty("description", Required = Required.Always)]
        [SwaggerSchema("Widget's description")]
        public string Description { get; set; } = null!;

        [JsonProperty("requires_auth", Required = Required.Always)]
        [SwaggerSchema("Indicates if authentication to the parent service is required in order to use the widget, **without checking if the user is already authenticated**")]
        public bool RequiresAuth { get; set; }

        [JsonIgnore]
        public int ServiceId { get; set; }

        [JsonProperty("service", Required = Required.Always)]
        [SwaggerSchema("The parent service")]
        public ServiceModel Service { get; set; } = null!;

        [JsonIgnore]
        public ICollection<UserWidgetModel> Users { get; set; } = null!;

        [JsonProperty("params", Required = Required.Always)]
        [SwaggerSchema("Widget's call parameters")]
        public ICollection<ParamModel> Params { get; set; } = null!;
    }
}