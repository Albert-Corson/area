using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace Area.API.Models.Widgets
{
    [SwaggerSchema("## This is an internal model of the `MicrosoftEvent` and `MicrosoftEmail` models")]
    public class MicrosoftPersonModel
    {
        [JsonProperty("name", Required = Required.Always)]
        [SwaggerSchema("Display name")]
        public string Name { get; set; } = null!;

        [JsonProperty("email", Required = Required.Always)]
        [SwaggerSchema("Email address")]
        public string Email { get; set; } = null!;
    }
}