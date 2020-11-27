using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using JwtConstants = Dashboard.API.Constants.JwtConstants;

namespace Dashboard.API.Services
{
    public class AuthService
    {
        private const string Algorithm = SecurityAlgorithms.HmacSha256;
        private readonly IConfiguration _configuration;
        private readonly TokenValidationParameters _validationParameters;
        private readonly JwtSecurityTokenHandler _tokenHandler = new JwtSecurityTokenHandler();

        public AuthService(IConfiguration configuration, TokenValidationParameters validationParameters)
        {
            _configuration = configuration;
            _validationParameters = validationParameters;
            _tokenHandler.InboundClaimTypeMap.Clear();
        }

        public string GenerateAccessToken(int userId)
        {
            return GenerateToken(DateTime.Now.AddTicks(JwtConstants.AccessTokenLifespan), new[] {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.AuthTime, DateTime.Now.Ticks.ToString()),
                new Claim(JwtRegisteredClaimNames.Typ, "access_token"),
            });
        }

        public string GenerateRefreshToken(int userId)
        {
            return GenerateToken(DateTime.Now.AddTicks(JwtConstants.RefreshTokenLifespan), new[] {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.AuthTime, DateTime.Now.Ticks.ToString()),
                new Claim(JwtRegisteredClaimNames.Typ, "refresh_token"),
            });
        }

        private string GenerateToken(DateTime expiryTime, IEnumerable<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SECRET_SALT"]));
            var signingCredentials = new SigningCredentials(key, Algorithm);
            var token = new JwtSecurityToken(
                issuer: _configuration["ValidIssuer"],
                audience: _configuration["ValidAudience"],
                claims: claims,
                notBefore: DateTime.Now,
                expires: expiryTime,
                signingCredentials: signingCredentials
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
                var nameId = claimsPrincipal.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Sub);
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
                var principal = _tokenHandler.ValidateToken(token, _validationParameters, out var validatedToken);
                return !IsJwtWithValidSecurityAlgorithm(validatedToken) ? null : principal;
            } catch {
                return null;
            }
        }

        private static bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
        {
            return (validatedToken is JwtSecurityToken jwtSecurityToken)
                   && jwtSecurityToken.Header.Alg.Equals(Algorithm, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
