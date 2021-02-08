using System.Collections.Generic;
using System.ComponentModel;
using Area.API.Models.Table.Owned;
using Area.API.Models.Widgets;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;

namespace Area.API.Models
{
    [SwaggerSubType(typeof(NewsApiArticleModel))]
    [SwaggerSubType(typeof(SpotifyTrackModel))]
    [SwaggerSubType(typeof(SpotifyArtistModel))]
    [SwaggerSchema("Base (generic) interpolation scheme for a widget's invocation result item. At least one of the field must be defined. This scheme can be extended depending on the widget")]
    public class WidgetCallResponseItemModel
    {
        [JsonProperty("header", Required = Required.DisallowNull)]
        [SwaggerSchema("A header/title")]
        public string? Header { get; set; }

        [JsonProperty("content", Required = Required.DisallowNull)]
        [SwaggerSchema("The content/description")]
        public string? Content { get; set; }

        [JsonProperty("link", Required = Required.DisallowNull)]
        [SwaggerSchema("A redirect link for the user")]
        public string? Link { get; set; }

        [JsonProperty("image", Required = Required.DisallowNull)]
        [SwaggerSchema("A link to an image")]
        public string? Image { get; set; }
    }

    public class WidgetCallResponseModel
    {
        public WidgetCallResponseModel(IEnumerable<WidgetParamModel> callParams)
        {
            CallParams = callParams;
        }

        [JsonProperty("params", Required = Required.Always)]
        [SwaggerSchema("List of parameters used for the request", ReadOnly = false)]
        public IEnumerable<WidgetParamModel> CallParams { get; }

        [JsonProperty("items", Required = Required.DisallowNull)]
        [SwaggerSchema("The result of the widget's invocation, if enumerable", ReadOnly = false)]
        public IEnumerable<WidgetCallResponseItemModel>? Items { get; set; }

        [JsonProperty("item", Required = Required.DisallowNull)]
        [ReadOnly(false)]
        [SwaggerSchema("The result of the widget's invocation, if singular")]
        public WidgetCallResponseItemModel? Item { get; set; }
    }
}