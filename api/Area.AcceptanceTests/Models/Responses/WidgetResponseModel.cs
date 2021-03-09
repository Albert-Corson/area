using System.Collections.Generic;
using Newtonsoft.Json;

namespace Area.AcceptanceTests.Models.Responses
{
    public class WidgetResponseItemModel
    {
        [JsonProperty("header", Required = Required.DisallowNull)]
        public string? Header { get; set; }

        [JsonProperty("content", Required = Required.DisallowNull)]
        public string? Content { get; set; }

        [JsonProperty("link", Required = Required.DisallowNull)]
        public string? Link { get; set; }

        [JsonProperty("image", Required = Required.DisallowNull)]
        public string? Image { get; set; }
    }

    public class WidgetResponseModel<TItem> where TItem : WidgetResponseItemModel
    {
        [JsonProperty("params", Required = Required.Always)]
        public IEnumerable<ParamModel> CallParams { get; set;  } = null!;

        [JsonProperty("items", Required = Required.Always)]
        public IEnumerable<TItem> Items { get; set; } = null!;
    }

    public class WidgetResponseModel : WidgetResponseModel<WidgetResponseItemModel>
    { }
}