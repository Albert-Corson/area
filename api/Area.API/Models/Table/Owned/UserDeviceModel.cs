using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Swashbuckle.AspNetCore.Annotations;
using Wangkanai.Detection.Models;
using Wangkanai.Detection.Services;

namespace Area.API.Models.Table.Owned
{
    [Owned]
    [Table("UserHasDevices")]
    public sealed class UserDeviceModel
    {
        public UserDeviceModel()
        { }

        public UserDeviceModel(IDetectionService detectionService, int userId, string? country = null)
        {
            UserId = userId;
            if (country != null)
                Country = country;
            Device = detectionService.Device.Type;
            Os = detectionService.Platform.Name;
            OsVersion = detectionService.Platform.Version.ToString();
            Architecture = detectionService.Platform.Processor;
            Browser = detectionService.Browser.Name;
            BrowserVersion = detectionService.Browser.Version.ToString();
            Id = (uint) GetHashCode();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [JsonProperty("id", Required = Required.Always)]
        [SwaggerSchema("Device's Id")]
        public uint Id { get; set; }

        [JsonIgnore]
        public int UserId { get; set; }

        [JsonIgnore]
        public long FirstUsed { get; set; } = DateTime.UtcNow.Ticks;

        [JsonProperty("last_used", Required = Required.Always)]
        [SwaggerSchema("Date in UTC Linux EPOCH at which the device was last used")]
        public long LastUsed { get; set; } = DateTime.UtcNow.Ticks;

        [JsonProperty("country", Required = Required.Always)]
        [SwaggerSchema("The country associated to the device")]
        public string Country { get; set; } = "Unknown";

        [JsonProperty("device", Required = Required.Always)]
        [JsonConverter(typeof(StringEnumConverter))]
        [SwaggerSchema("Device's type")]
        public Device Device { get; set; }

        [JsonProperty("browser", Required = Required.Always)]
        [JsonConverter(typeof(StringEnumConverter))]
        [SwaggerSchema("Browser's type")]
        public Browser Browser { get; set; }

        [JsonProperty("browser_version", Required = Required.Always)]
        [SwaggerSchema("Browser's version")]
        public string BrowserVersion { get; set; } = null!;

        [JsonProperty("os", Required = Required.Always)]
        [JsonConverter(typeof(StringEnumConverter))]
        [SwaggerSchema("Device's operating system")]
        public Platform Os { get; set; }

        [JsonProperty("os_version", Required = Required.Always)]
        [SwaggerSchema("Operating system's version")]
        public string OsVersion { get; set; } = null!;

        [JsonProperty("architecture", Required = Required.Always)]
        [JsonConverter(typeof(StringEnumConverter))]
        [SwaggerSchema("Device's processor architecture")]
        public Processor Architecture { get; set; }

        public override int GetHashCode()
        {
            var str = ToString();

            unchecked {
                var hash1 = (5381 << 16) + 5381;
                var hash2 = hash1;

                for (var i = 0; i < str.Length; i += 2) {
                    hash1 = ((hash1 << 5) + hash1) ^ str[i];
                    if (i == str.Length - 1)
                        break;
                    hash2 = ((hash2 << 5) + hash2) ^ str[i + 1];
                }

                return hash1 + (hash2 * 1566083941);
            }
        }

        public override string ToString()
        {
            return UserId +
                Country +
                Device +
                Os +
                OsVersion +
                Architecture +
                Browser +
                BrowserVersion;
        }

        public override bool Equals(object? obj)
        {
            if (obj is UserDeviceModel)
                return obj.ToString() == ToString();
            return false;
        }
    }
}