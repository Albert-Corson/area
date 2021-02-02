using System.Collections.Generic;
using Area.API.Models.Table.Owned;
using Newtonsoft.Json;

namespace Area.API.Models
{
    public class WidgetCallResponseItemModel
    {
        [JsonProperty("header")]
        public string? Header { get; set; }

        [JsonProperty("content")]
        public string? Content { get; set; }

        [JsonProperty("link")]
        public string? Link { get; set; }

        [JsonProperty("image")]
        public string? Image { get; set; }
    }

    public class WidgetCallResponseModel
    {
        public WidgetCallResponseModel(IEnumerable<WidgetParamModel> callParams)
        {
            CallParams = callParams;
        }

        [JsonProperty("params")]
        public IEnumerable<WidgetParamModel> CallParams { get; }

        [JsonProperty("items")]
        public IEnumerable<WidgetCallResponseItemModel>? Items { get; set; }

        [JsonProperty("item")]
        public WidgetCallResponseItemModel? Item { get; set; }
    }
}
