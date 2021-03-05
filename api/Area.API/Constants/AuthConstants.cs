using System;

namespace Area.API.Constants
{
    public static class AuthConstants
    {
        public const string SecretKeyName = "SECRET_SALT";
        public const string ValidAudience = "ValidAudience";
        public const long CodeLifespanTicks = TimeSpan.TicksPerSecond * 60;
        public const long AccessTokenLifespanTicks = TimeSpan.TicksPerDay * 2;
        public const long AccessTokenLifespanSeconds = AccessTokenLifespanTicks / TimeSpan.TicksPerSecond;
        public const long RefreshTokenLifespanTicks = TimeSpan.TicksPerDay * 14;

        public static class IpData
        {
            public const string Base = nameof(IpData);
            public const string Key = Base + ":" + nameof(Key);
        }

        public static class Facebook
        {
            public const string Base = nameof(Facebook);
            public const string ClientId = Base + ":" + nameof(ClientId);
            public const string ClientSecret = Base + ":" + nameof(ClientSecret);
            public const string RedirectUri = Base + ":" + nameof(RedirectUri);
        }

        public static class Google
        {
            public const string Base = nameof(Google);
            public const string ClientId = Base + ":" + nameof(ClientId);
            public const string ClientSecret = Base + ":" + nameof(ClientSecret);
            public const string RedirectUri = Base + ":" + nameof(RedirectUri);
        }

        public static class Microsoft
        {
            public const string Base = nameof(Microsoft);
            public const string ClientId = Base + ":" + nameof(ClientId);
            public const string ClientSecret = Base + ":" + nameof(ClientSecret);
            public const string AuthRedirectUri = Base + ":" + nameof(AuthRedirectUri);
        }

        public static class Imgur
        {
            public const string Base = nameof(Imgur);
            public const string ClientId = Base + ":" + nameof(ClientId);
            public const string ClientSecret = Base + ":" + nameof(ClientSecret);
        }

        public static class Spotify
        {
            public const string Base = nameof(Spotify);
            public const string ClientId = Base + ":" + nameof(ClientId);
            public const string ClientSecret = Base + ":" + nameof(ClientSecret);
            public const string RedirectUri = Base + ":" + nameof(RedirectUri);
        }

        public static class NewsApi
        {
            public const string Base = nameof(NewsApi);
            public const string Key = Base + ":" + nameof(Key);
        }

        public static class CatApi
        {
            public const string Base = nameof(CatApi);
            public const string Key = Base + ":" + nameof(Key);
        }

    }
}