using System;
using Newtonsoft.Json;

namespace Area.AcceptanceTests.Models.Requests
{
    public class ExternalAuthModel
    {
        [JsonProperty("redirect_url")]
        public string RedirectUrl { get; set; } = null!;

        [JsonProperty("state")]
        public string? State { get; set; }

        // TODO: enable these
        // [JsonProperty("client_id")]
        // public string ClientId { get; set; } = null!;
    }
}