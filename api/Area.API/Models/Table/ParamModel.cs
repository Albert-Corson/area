using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using Area.API.Models.Table.ManyToMany;
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

        public ParamModel(ParamModel other)
        {
            Id = other.Id;
            Name = other.Name;
            Type = other.Type;
            WidgetId = other.WidgetId;
            Value = other.Value;
            Widget = other.Widget;
            Enums = other.Enums;
            ConvertedValue = other.ConvertedValue;
        }

        public ParamModel(UserParamModel userParam)
            : this(userParam.Param) =>
            Value = userParam.Value;

        public enum ParamType
        {
            [EnumMember(Value = "string")] String,
            [EnumMember(Value = "integer")]  Integer,
            [EnumMember(Value = "boolean")] Boolean,
            [EnumMember(Value = "enum")] Enum
        }

        [ForeignKey("ParamId")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonIgnore]
        public int Id { get; set; }

        [JsonProperty("name", Required = Required.Always)]
        [SwaggerSchema("Parameter's name")]
        public string Name { get; set; } = null!;

        [JsonProperty("type", Required = Required.Always)]
        [SwaggerSchema("Parameter's type, either int or string")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ParamType Type { get; set; }

        [JsonProperty("value", Required = Required.DisallowNull)]
        [SwaggerSchema("Parameter's default value")]
        public string? Value { get; set; }

        [JsonProperty("allowed_values", Required = Required.DisallowNull)]
        [SwaggerSchema("Allowed values for \"enum\" type parameters")]
        private IEnumerable<EnumValueModel>? AllowedValues => Enums.Count == 0 ? null : Enums.SelectMany(model => model.Enum.Values);

        [JsonIgnore]
        public int WidgetId { get; set; }

        [JsonIgnore]
        public WidgetModel Widget { get; set; } = null!;

        [JsonIgnore]
        public ICollection<ParamEnumModel> Enums { get; set; } = null!;

        [NotMapped]
        [JsonIgnore]
        public object? ConvertedValue;
    }
}