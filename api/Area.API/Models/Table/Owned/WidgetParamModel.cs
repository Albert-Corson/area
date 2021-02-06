using System.ComponentModel.DataAnnotations.Schema;
using Area.API.Constants;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace Area.API.Models.Table.Owned
{
    [Owned]
    [Table("WidgetHasParams")]
    public class WidgetParamModel
    {
        [JsonProperty("name", Required = Newtonsoft.Json.Required.Always)]
        [SwaggerSchema("Parameter's name")]
        public string Name { get; set; } = null!;

        [JsonProperty("type", Required = Newtonsoft.Json.Required.Always)]
        [SwaggerSchema("Parameter's type, either int or string")]
        public string Type { get; set; } = null!;

        [JsonProperty("value", Required = Newtonsoft.Json.Required.DisallowNull)]
        [SwaggerSchema("Parameter's default value (applicable to non `required` parameters only)")]
        public string? Value { get; set; }

        [JsonProperty("required", Required = Newtonsoft.Json.Required.Always)]
        [SwaggerSchema("Indicates if the parameter has to defined at for each call to the widget (`"
            + RoutesConstants.Widgets.CallWidget + "`)")]
        public virtual bool Required { get; set; }
    }
}