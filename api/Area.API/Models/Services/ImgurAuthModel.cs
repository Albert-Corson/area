using Newtonsoft.Json;

namespace Area.API.Models.Services
{
    public class ImgurAuthModel
    {
        [JsonProperty("access_token", Required = Required.Always)]
        public string AccessToken { get; set; } = null!;

        [JsonProperty("account_id", Required = Required.Always)]
        public string AccountId { get; set; } = null!;

        [JsonProperty("account_username", Required = Required.Always)]
        public string AccountUsername { get; set; } = null!;

        [JsonProperty("expires_in", Required = Required.Always)]
        public int ExpiresIn { get; set; }

        [JsonProperty("refresh_token", Required = Required.Always)]
        public string RefreshToken { get; set; } = null!;

        [JsonProperty("token_type", Required = Required.Always)]
        public string TokenType { get; set; } = null!;

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.None, new JsonSerializerSettings {
                NullValueHandling = NullValueHandling.Ignore
            });
        }
    }
}