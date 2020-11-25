using Newtonsoft.Json;

namespace Dashboard.API.Models.Response
{
    public class UserTokenModel
    {
        public UserTokenModel()
        { }

        public UserTokenModel(string accessToken = "", long expiresIn = 0, string? refreshToken = null)
        {
            AccessToken = accessToken;
            ExpiresIn = expiresIn;
            RefreshToken = refreshToken;
        }

        [JsonProperty("access_token")]
        public string AccessToken { get; set; } = "";

        [JsonProperty("expires_in")]
        public long ExpiresIn { get; set; }

        [JsonProperty("refresh_token")]
        public string? RefreshToken { get; set; }
    }
}
