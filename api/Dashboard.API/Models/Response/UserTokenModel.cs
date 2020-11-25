using Newtonsoft.Json;

namespace Dashboard.API.Models.Response
{
    public class UserTokenModel
    {
        public UserTokenModel()
        { }

        public UserTokenModel(string refreshToken = "", string accessToken = "", long expiresIn = 0)
        {
            RefreshToken = refreshToken;
            AccessToken = accessToken;
            ExpiresIn = expiresIn;
        }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; } = "";

        [JsonProperty("access_token")]
        public string AccessToken { get; set; } = "";

        [JsonProperty("expires_in")]
        public long ExpiresIn { get; set; }
    }
}
