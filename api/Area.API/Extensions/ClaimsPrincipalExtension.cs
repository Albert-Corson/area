using System.Security.Claims;
using Area.API.Models.Table;
using Area.API.Repositories;
using Area.API.Services;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Area.API.Extensions
{
    public static class ClaimsPrincipalExtension
    {
        public static bool TryGetUser(this ClaimsPrincipal principal, UserRepository userRepository, out UserModel user)
        {
            user = null!;

            if (!principal.TryGetUserId(out var userId))
                return false;

            var nullableUser = userRepository.GetUser(userId);
            user = nullableUser!;

            return nullableUser != null;
        }

        public static bool TryGetUserId(this ClaimsPrincipal principal, out int userId)
        {
            userId = 0;

            var userIdClaim = principal.FindFirst(claim => claim.Type == AuthService.ClaimTypeUserId);

            return userIdClaim != null && int.TryParse(userIdClaim.Value, out userId);
        }

        public static bool TryGetDeviceId(this ClaimsPrincipal principal, out uint deviceId)
        {
            deviceId = 0;

            var deviceIdClaim = principal.FindFirst(claim => claim.Type == JwtRegisteredClaimNames.Iss);

            return deviceIdClaim != null && uint.TryParse(deviceIdClaim.Value, out deviceId);
        }

        public static bool TryGetAuthTime(this ClaimsPrincipal principal, out long ticks)
        {
            ticks = 0;

            var authTimeClaim = principal.FindFirst(claim => claim.Type == JwtRegisteredClaimNames.AuthTime);

            return authTimeClaim != null && long.TryParse(authTimeClaim.Value, out ticks);
        }

        public static bool TryGetExpiry(this ClaimsPrincipal principal, out long expiry)
        {
            expiry = 0;

            var authTimeClaim = principal.FindFirst(claim => claim.Type == JwtRegisteredClaimNames.Exp);

            return authTimeClaim != null && long.TryParse(authTimeClaim.Value, out expiry);
        }
    }
}