using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using JwtConstants = Area.API.Constants.JwtConstants;

namespace Area.API.Utilities
{
    public class AuthUtilities
    {
        private const string ClaimTypeUserId = "uid";
        private const string Algorithm = SecurityAlgorithms.HmacSha256;
        private readonly IConfiguration _configuration;
        private readonly TokenValidationParameters _validationParameters;

        public AuthUtilities(IConfiguration configuration, TokenValidationParameters validationParameters)
        {
            _configuration = configuration;
            _validationParameters = validationParameters;
        }

        public static int? GetUserIdFromPrincipal(ClaimsPrincipal principal)
        {
            var userIdClaim = principal.FindFirst(claim => claim.Type == ClaimTypeUserId);

            if (userIdClaim == null)
                return null;
            if (!int.TryParse(userIdClaim.Value, out var userId))
                return null;
            return userId;
        }

        public string GenerateAccessToken(int userId)
        {
            return GenerateToken(DateTime.Now.AddTicks(JwtConstants.AccessTokenLifespan), new[] {
                new Claim(ClaimTypeUserId, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.AuthTime, DateTime.Now.Ticks.ToString()),
                new Claim(JwtRegisteredClaimNames.Typ, "access_token")
            });
        }

        public string GenerateRefreshToken(int userId)
        {
            return GenerateToken(DateTime.Now.AddTicks(JwtConstants.RefreshTokenLifespan), new[] {
                new Claim(ClaimTypeUserId, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.AuthTime, DateTime.Now.Ticks.ToString()),
                new Claim(JwtRegisteredClaimNames.Typ, "refresh_token")
            });
        }

        public static bool IsValidEmail(string email)
        {
            try {
                var addr = new MailAddress(email);
                return addr.Address == email;
            } catch {
                return false;
            }
        }

        private string GenerateToken(DateTime expiryTime, IEnumerable<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SECRET_SALT"]));
            var signingCredentials = new SigningCredentials(key, Algorithm);
            var token = new JwtSecurityToken(
                _configuration["ValidIssuer"],
                _configuration["ValidAudience"],
                claims,
                DateTime.Now,
                expiryTime,
                signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public int? GetUserIdFromRefreshToken(string refreshToken)
        {
            var claimsPrincipal = GetPrincipalFromToken(refreshToken);

            if (claimsPrincipal == null)
                return null;
            try {
                var typ = claimsPrincipal.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Typ);
                var nameId = claimsPrincipal.Claims.First(claim => claim.Type == ClaimTypeUserId);
                if (typ.Value != "refresh_token" || nameId == null)
                    return null;
                return int.Parse(nameId.Value);
            } catch {
                return null;
            }
        }

        private ClaimsPrincipal? GetPrincipalFromToken(string token)
        {
            try {
                var principal = new JwtSecurityTokenHandler().ValidateToken(token, _validationParameters, out var validatedToken);
                return !IsJwtWithValidSecurityAlgorithm(validatedToken) ? null : principal;
            } catch {
                return null;
            }
        }

        private static bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
        {
            return validatedToken is JwtSecurityToken jwtSecurityToken
                && jwtSecurityToken.Header.Alg.Equals(Algorithm, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}