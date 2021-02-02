using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Area.API.Models.Services
{
    public class SpotifyAuthModel
    {
        [JsonProperty("access_token", Required = Required.Always)]
        public string AccessToken { get; set; } = null!;

        [JsonProperty("token_type", Required = Required.Always)]
        public string TokenType { get; set; } = null!;

        [JsonProperty("expires_in", Required = Required.Always)]
        public int ExpiresIn { get; set; }

        [JsonProperty("scope", Required = Required.Always)]
        public string Scope { get; set; } = null!;

        [JsonProperty("refresh_token", Required = Required.Always)]
        public string RefreshToken { get; set; } = null!;

        [JsonProperty("created_at", Required = Required.Always)]
        public DateTime CreatedAt { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.None, new JsonSerializerSettings {
                NullValueHandling = NullValueHandling.Ignore
            });
        }
    }
}
