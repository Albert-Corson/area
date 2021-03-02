using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace Area.API.Models.Table.Owned
{
    [Owned]
    [Table("EnumHasValues")]
    public class EnumValueModel
    {
        [JsonProperty("value", Required = Required.Always)]
        [SwaggerSchema("Value to send in a widget call")]
        public string Value { get; set; } = null!;

        [JsonProperty("display_name", Required = Required.Always)]
        [SwaggerSchema("Human readable name of the value")]
        public string DisplayName { get; set; } = null!;
    }
}