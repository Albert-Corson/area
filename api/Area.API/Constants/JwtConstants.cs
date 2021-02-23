using System;

namespace Area.API.Constants
{
    public static class JwtConstants
    {
        public const string SecretKeyName = "SECRET_SALT";
        public const string ValidAudience = "ValidAudience";
        public const long AccessTokenLifespanTicks = TimeSpan.TicksPerDay * 2;
        public const long AccessTokenLifespanSeconds = AccessTokenLifespanTicks / TimeSpan.TicksPerSecond;
        public const long RefreshTokenLifespanTicks = TimeSpan.TicksPerDay * 14;
    }
}