using System;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace Area.API.Models
{
    public class ExternalAuthModel
    {
        [JsonProperty("redirect_url", Required = Required.Always)]
        [SwaggerSchema("The URL to redirect the user to once the operation is completed")]
        public Uri RedirectUrl { get; set; } = null!;

        [JsonProperty("state", Required = Required.Default)]
        [SwaggerSchema("A freely-defined value that will sent back to the client")]
        public string? State { get; set; }

        // TODO: enable these
        // [JsonProperty("client_id", Required = Required.Always)]
        // [SwaggerSchema("Id of the client making the request")]
        // public string ClientId { get; set; } = null!;
    }
}