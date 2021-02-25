using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Area.API.Extensions;
using Area.API.Models.Table;
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
        public const string ClaimTypeUserId = "uid";
        private const string Algorithm = SecurityAlgorithms.HmacSha256;
        private readonly IConfiguration _configuration;
        private readonly TokenValidationParameters _validationParameters;
        private readonly IDetectionService _detection;
        private readonly IpDataClient _ipDataClient;
        private readonly UserRepository _userRepository;

        public AuthService(IConfiguration configuration, TokenValidationParameters validationParameters,
            IDetectionService detection, IpDataClient ipDataClient, UserRepository userRepository)
        {
            _configuration = configuration;
            _validationParameters = validationParameters;
            _detection = detection;
            _ipDataClient = ipDataClient;
            _userRepository = userRepository;
        }

        public async Task<string> GenerateAccessToken(int userId, IPAddress ipAddress)
        {
            var claims = new List<Claim>(new[] {
                new Claim(JwtRegisteredClaimNames.Typ, "access_token")
            });

            return await GenerateToken(DateTime.Now.AddTicks(JwtConstants.AccessTokenLifespanTicks), claims, userId,
                ipAddress);
        }

        public async Task<string> GenerateRefreshToken(int userId, IPAddress ipAddress)
        {
            var claims = new List<Claim>(new[] {
                new Claim(JwtRegisteredClaimNames.Typ, "refresh_token")
            });

            return await GenerateToken(DateTime.Now.AddTicks(JwtConstants.RefreshTokenLifespanTicks), claims, userId,
                ipAddress);
        }

        private async Task<string> GenerateToken(DateTime expiryTime, List<Claim> claims, int userId,
            IPAddress ipAddress)
        {
            var country = await _ipDataClient.GetCountry(ipAddress);
            var device = new UserDeviceModel(_detection, userId, country);

            AssociateDeviceToUser(userId, device);

            claims.AddRange(new[] {
                new Claim(ClaimTypeUserId, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.AuthTime, device.FirstUsed.ToString())
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

        public async Task<bool> ValidateDeviceUse(ClaimsPrincipal principal, UserModel user, IPAddress ipv4)
        {
            if (!principal.TryGetAuthTime(out var authTime)
                || !principal.TryGetDeviceId(out var registeredDeviceId))
                return false;

            var registeredDevice = user.Devices.FirstOrDefault(model => model.Id == registeredDeviceId);
            if (registeredDevice == null || registeredDevice.FirstUsed > authTime)
                return false;

            var currentCountry = await _ipDataClient.GetCountry(ipv4);
            var currentDevice = new UserDeviceModel(_detection, user.Id, currentCountry);
            if (registeredDeviceId != currentDevice.Id)
                return false;

            registeredDevice.LastUsed = DateTime.Now.Ticks;

            return true;
        }

        private async Task<bool> ValidateDeviceUse(ClaimsPrincipal principal, IPAddress ipv4)
        {
            var user = principal.TryGetUserId(out var userId)
                ? _userRepository.GetUser(userId, asNoTracking: false)
                : null;
            if (user == null)
                return false;

            return await ValidateDeviceUse(principal, user, ipv4);
        }

        public async Task<ClaimsPrincipal?> ValidateRefreshToken(string refreshToken, IPAddress ipv4)
        {
            if (!TryGetPrincipalFromToken(refreshToken, out var claimsPrincipal))
                return null;

            try {
                var typ = claimsPrincipal.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Typ);
                if (typ.Value != "refresh_token")
                    return null;

                if (await ValidateDeviceUse(claimsPrincipal, ipv4))
                    return claimsPrincipal;
                return null;
            } catch {
                return null;
            }
        }

        private bool TryGetPrincipalFromToken(string token, out ClaimsPrincipal principals)
        {
            try {
                principals =
                    new JwtSecurityTokenHandler().ValidateToken(token, _validationParameters, out var validatedToken);
                return IsJwtWithValidSecurityAlgorithm(validatedToken);
            } catch {
                principals = new ClaimsPrincipal();
                return false;
            }
        }

        private static bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
        {
            return validatedToken is JwtSecurityToken jwtSecurityToken
                && jwtSecurityToken.Header.Alg.Equals(Algorithm, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}