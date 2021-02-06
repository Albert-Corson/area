using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace Area.API.Models.Request
{
    public class RegisterModel
    {
        [JsonProperty("username", Required = Required.Always)]
        [SwaggerSchema("The user's username")]
        public string Username { get; set; } = null!;

        [JsonProperty("password", Required = Required.Always)]
        [SwaggerSchema("The user's password")]
        public string Password { get; set; } = null!;

        [JsonProperty("email", Required = Required.Always)]
        [SwaggerSchema("The user's email address")]
        public string Email { get; set; } = null!;
    }
}