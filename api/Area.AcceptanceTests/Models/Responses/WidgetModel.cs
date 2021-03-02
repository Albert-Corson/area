using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Area.AcceptanceTests.Models.Responses
{
    public class WidgetModel : AboutDotJsonModel.WidgetModel
    {
        private bool Equals(WidgetModel other) => base.Equals(other) && Frequency == other.Frequency && Service.Equals(other.Service) && Params.Equals(other.Params);

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            return obj.GetType() == GetType() && Equals((WidgetModel) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Frequency, Service, Params);
        }

        public void Copy(WidgetModel other)
        {
            base.Copy(other);
            Frequency = other.Frequency;
            Service.Copy(other.Service);
            Params = other.Params;
        }

        public static bool operator!=(WidgetModel self, WidgetModel other)
        {
            return !(self == other);
        }

        public static bool operator==(WidgetModel self, WidgetModel other)
        {
            if (self.Params.Count != other.Params.Count)
                return false;

            return self as AboutDotJsonModel.WidgetModel == other
                && self.Frequency == other.Frequency
                && self.Service == other.Service;
        }

        [JsonProperty("frequency", Required = Required.Always)]
        public int Frequency { get; set; }

        [JsonProperty("service", Required = Required.Always)]
        public ServiceModel Service { get; set; } = null!;

        [JsonProperty("params", Required = Required.Always)]
        public ICollection<ParamModel> Params { get; set; } = null!;
    }
}