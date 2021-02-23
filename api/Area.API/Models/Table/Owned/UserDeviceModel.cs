using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Wangkanai.Detection.Models;
using Wangkanai.Detection.Services;

namespace Area.API.Models.Table.Owned
{
    [Owned]
    [Table("UserHasDevice")]
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
            Id = GetHashCode();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [JsonIgnore]
        public int Id { get; set; }

        [JsonIgnore]
        public int UserId { get; set; }
        
        [JsonIgnore]
        public DateTime FirstUsed { get; set; } = DateTime.Now;

        [JsonProperty("last_used", Required = Required.Always)]
        public DateTime LastUsed { get; set; } = DateTime.Now;

        [JsonProperty("country", Required = Required.Always)]
        public string Country { get; set; } = "Local network";

        [JsonProperty("device", Required = Required.Always)]
        [JsonConverter(typeof(StringEnumConverter))]
        public Device Device { get; set; }

        [JsonProperty("browser", Required = Required.DisallowNull)]
        [JsonConverter(typeof(BrowserJsonConverter))]
        public Browser Browser { get; set; }

        [JsonProperty("browser_version", Required = Required.DisallowNull)]
        public string BrowserVersion { get; set; } = null!;

        [JsonProperty("os", Required = Required.Always)]
        [JsonConverter(typeof(StringEnumConverter))]
        public Platform Os { get; set; }

        [JsonProperty("os_version", Required = Required.Always)]
        public string OsVersion { get; set; } = null!;

        [JsonProperty("architecture", Required = Required.Always)]
        [JsonConverter(typeof(StringEnumConverter))]
        public Processor Architecture { get; set; }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
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

    internal class BrowserJsonConverter : JsonConverter<Browser>
    {
        public override void WriteJson(JsonWriter writer, Browser value, JsonSerializer serializer)
        {
            if (value == Browser.Unknown)
                writer.WriteNull();
            writer.WriteValue(value.ToString());
        }

        public override Browser ReadJson(JsonReader reader, Type objectType, Browser existingValue,
            bool hasExistingValue,
            JsonSerializer serializer)
        {
            return Enum.TryParse<Browser>(reader.Value as string, true, out var value) ? value : Browser.Unknown;
        }
    }
}