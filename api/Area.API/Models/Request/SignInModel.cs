using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace Area.API.Models.Request
{
    public class SignInModel
    {
        [JsonProperty("identifier", Required = Required.Always)]
        [SwaggerSchema("Username or email")]
        public string Identifier { get; set; } = null!;

        [JsonProperty("password", Required = Required.Always)]
        [SwaggerSchema("User's password")]
        public string Password { get; set; } = null!;
    }
}