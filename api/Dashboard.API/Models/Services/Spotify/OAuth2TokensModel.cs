using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Dashboard.API.Models.Services.Spotify
{
    public class OAuth2TokensModel
    {
        [Required]
        [JsonProperty("access_token")]
        public string? AccessToken { get; set; }

        [Required]
        [JsonProperty("token_type")]
        public string? TokenType { get; set; }

        [Required]
        [JsonProperty("expires_in")]
        public int? ExpiresIn { get; set; }

        [Required]
        [JsonProperty("scope")]
        public string? Scope { get; set; }

        [Required]
        [JsonProperty("refresh_token")]
        public string? RefreshToken { get; set; }

        [Required]
        [JsonProperty("created_at")]
        public DateTime? CreatedAt { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.None, new JsonSerializerSettings {
                NullValueHandling = NullValueHandling.Ignore
            });
        }
    }
}
