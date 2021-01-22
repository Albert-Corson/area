using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Area.API.Models.Services
{
    public class ImgurAuthModel
    {
        [Required]
        [JsonProperty("access_token")]
        public string? AccessToken { get; set; }

        [Required]
        [JsonProperty("account_id")]
        public string? AccountId { get; set; }

        [Required]
        [JsonProperty("account_username")]
        public string? AccountUsername { get; set; }

        [Required]
        [JsonProperty("expires_in")]
        public int? ExpiresIn { get; set; }

        [Required]
        [JsonProperty("refresh_token")]
        public string? RefreshToken { get; set; }

        [Required]
        [JsonProperty("token_type")]
        public string? TokenType { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.None, new JsonSerializerSettings {
                NullValueHandling = NullValueHandling.Ignore
            });
        }
    }
}
