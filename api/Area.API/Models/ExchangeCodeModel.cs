using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace Area.API.Models
{
    public class ExchangeCodeModel
    {
        [JsonProperty("code", Required = Required.Always)]
        [SwaggerSchema("Authentication code to be exchanged for a pair of access and refresh token")]
        public string Code { get; set; } = null!;

        // TODO: enable these
        // [JsonProperty("client_id", Required = Required.Always)]
        // [SwaggerSchema("Id of the client making the request")]
        // public string ClientId { get; set; } = null!;
        //
        // [JsonProperty("client_secret", Required = Required.Always)]
        // [SwaggerSchema("Secret of the client making the request")]
        // public string ClientSecret { get; set; } = null!;
    }
}