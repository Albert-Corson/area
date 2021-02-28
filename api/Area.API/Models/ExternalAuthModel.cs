using System;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace Area.API.Models
{
    public class ExternalAuthModel
    {
        [JsonProperty("redirect_url", Required = Required.Always)]
        [SwaggerSchema("The URL to redirect the user to once the operation is completed")]
        [JsonConverter(typeof(UriBuilderJsonConverter))]
        public UriBuilder RedirectUrl { get; set; } = null!;

        [JsonProperty("state", Required = Required.Default)]
        [SwaggerSchema("A freely-defined value that will sent back to the client")]
        public string? State { get; set; }

        // TODO: enable these
        // [JsonProperty("client_id", Required = Required.Always)]
        // [SwaggerSchema("Id of the client making the request")]
        // public string ClientId { get; set; } = null!;
    }

    internal class UriBuilderJsonConverter : JsonConverter<UriBuilder>
    {
        public override void WriteJson(JsonWriter writer, UriBuilder value, JsonSerializer serializer) =>
            writer.WriteValue(value.ToString());

        public override UriBuilder ReadJson(JsonReader reader, Type objectType, UriBuilder existingValue, bool hasExistingValue, JsonSerializer serializer) =>
            new UriBuilder((reader.Value as string)!);
    }
}