using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Area.API.Extensions;
using Area.API.Models.Table.Owned;
using Area.API.Repositories;
using IpData;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Wangkanai.Detection.Services;
using JwtConstants = Area.API.Constants.JwtConstants;

namespace Area.API.Services
{
    public class AuthService
    {
        private const string ClaimTypeUserId = "uid";
        private const string Algorithm = SecurityAlgorithms.HmacSha256;
        private readonly IConfiguration _configuration;
        private readonly TokenValidationParameters _validationParameters;
        private readonly IDetectionService _detection;
        private readonly IpDataClient _ipDataClient;
        private readonly UserRepository _userRepository;

        public AuthService(IConfiguration configuration, TokenValidationParameters validationParameters, IDetectionService detection, IpDataClient ipDataClient, UserRepository userRepository)
        {
            _configuration = configuration;
            _validationParameters = validationParameters;
            _detection = detection;
            _ipDataClient = ipDataClient;
            _userRepository = userRepository;
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

        public static int? GetDeviceIdFromPrincipal(ClaimsPrincipal principal)
        {
            var deviceIdClaim = principal.FindFirst(claim => claim.Type == JwtRegisteredClaimNames.Iss);

            if (deviceIdClaim == null)
                return null;
            if (!int.TryParse(deviceIdClaim.Value, out var deviceId))
                return null;
            return deviceId;
        }

        public async Task<string> GenerateAccessToken(int userId, IPAddress ipAddress)
        {
            var claims = new List<Claim>(new [] {
                new Claim(JwtRegisteredClaimNames.Typ, "access_token")
            });

            return await GenerateToken(DateTime.Now.AddTicks(JwtConstants.AccessTokenLifespanTicks), claims, userId, ipAddress);
        }

        public async Task<string> GenerateRefreshToken(int userId, IPAddress ipAddress)
        {
            var claims = new List<Claim>(new [] {
                new Claim(JwtRegisteredClaimNames.Typ, "refresh_token")
            });

            return await GenerateToken(DateTime.Now.AddTicks(JwtConstants.RefreshTokenLifespanTicks), claims, userId, ipAddress);
        }

        private async Task<string> GenerateToken(DateTime expiryTime, List<Claim> claims, int userId, IPAddress ipAddress)
        {
            var country = await _ipDataClient.GetCountry(ipAddress);
            var device = new UserDeviceModel(_detection, userId, country);

            AssociateDeviceToUser(userId, device);

            claims.AddRange(new [] {
                new Claim(ClaimTypeUserId, userId.ToString()),
            });

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration[JwtConstants.SecretKeyName]));
            var signingCredentials = new SigningCredentials(key, Algorithm);
            var token = new JwtSecurityToken(
                device.Id.ToString(),
                _configuration[JwtConstants.ValidAudience],
                claims,
                DateTime.Now,
                expiryTime,
                signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private void AssociateDeviceToUser(int userId, UserDeviceModel device)
        {
            var user = _userRepository.GetUser(userId, asNoTracking: false);

            var existingDevice = user!.Devices.FirstOrDefault(model => model.Id == device.Id);

            if (existingDevice != null) {
                existingDevice.LastUsed = device.LastUsed;
            } else {
                user.Devices.Add(device);
            }
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