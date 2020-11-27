using System;

namespace Dashboard.API.Constants
{
    public static class JwtConstants
    {
        public const string SecretKeyName = "SECRET_SALT";

        public const string ValidIssuer = "ValidIssuer";

        public const string ValidAudience = "ValidAudience";

        public const long AccessTokenLifespan = TimeSpan.TicksPerDay * 2;

        public const long RefreshTokenLifespan = TimeSpan.TicksPerDay * 14;
    }
}
