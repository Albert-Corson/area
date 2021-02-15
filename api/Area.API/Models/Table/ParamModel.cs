using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Area.API.Constants;
using Area.API.Models.Table.Owned;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Swashbuckle.AspNetCore.Annotations;

namespace Area.API.Models.Table
{
    [Table("Params")]
    public class ParamModel
    {
        public ParamModel()
        { }

        public ParamModel(UserParamModel userParam)
        {
            Id = userParam.Param.Id;
            Name = userParam.Param.Name;
            Type = userParam.Param.Type;
            Required = userParam.Param.Required;
            WidgetId = userParam.Param.WidgetId;
            Value = userParam.Value;
        }

        public enum ParamType
        {
            [EnumMember(Value = "string")] String,
            [EnumMember(Value = "integer")]  Integer,
            [EnumMember(Value = "boolean")] Boolean
        }

        [ForeignKey("ParamId")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonIgnore]
        public int Id { get; set; }

        [JsonProperty("name", Required = Newtonsoft.Json.Required.Always)]
        [SwaggerSchema("Parameter's name")]
        public string Name { get; set; } = null!;

        [JsonProperty("type", Required = Newtonsoft.Json.Required.Always)]
        [SwaggerSchema("Parameter's type, either int or string")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ParamType Type { get; set; }

        [JsonProperty("value", Required = Newtonsoft.Json.Required.DisallowNull)]
        [SwaggerSchema("Parameter's default value (applicable to non `required` parameters only)")]
        public string? Value { get; set; }

        [JsonProperty("required", Required = Newtonsoft.Json.Required.Always)]
        [SwaggerSchema("Indicates if the parameter has to defined at for each call to the widget (`"
            + RoutesConstants.Widgets.CallWidget + "`)")]
        public bool Required { get; set; }

        [JsonIgnore]
        public int WidgetId { get; set; }

        [JsonIgnore]
        public WidgetModel Widget { get; set; } = null!;
    }
}