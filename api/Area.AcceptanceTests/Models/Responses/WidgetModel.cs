using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Area.AcceptanceTests.Models.Responses
{
    public class WidgetModel : AboutDotJsonModel.WidgetModel
    {
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

            foreach (var param in self.Params) {
                var otherParam = other.Params.FirstOrDefault(model => model.Name == param.Name);
                if (otherParam != param)
                    return false;
            }
            
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