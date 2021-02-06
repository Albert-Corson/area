using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SpotifyAPI.Web;
using Swashbuckle.AspNetCore.Annotations;

namespace Area.API.Models.Widgets
{
    public class SpotifyTrackModel : WidgetCallResponseItemModel
    {
        public SpotifyTrackModel()
        { }

        public SpotifyTrackModel(FullTrack track)
        {
            Image = track.Album.Images.FirstOrDefault()?.Url;
            Header = track.Name;
            if (track.ExternalUrls.TryGetValue("spotify", out var link))
                Link = link;
            else if (track.ExternalUrls.Count > 0)
                Link = track.ExternalUrls.FirstOrDefault().Value;

            Artists = track.Artists.Select(artist => artist.Name);
            Popularity = track.Popularity;
            Preview = track.PreviewUrl;
        }

        public SpotifyTrackModel(SimpleTrack track)
        {
            Header = track.Name;
            if (track.ExternalUrls.TryGetValue("spotify", out var link))
                Link = link;
            else if (track.ExternalUrls.Count > 0)
                Link = track.ExternalUrls.FirstOrDefault().Value;

            Artists = track.Artists.Select(artist => artist.Name);
            Preview = track.PreviewUrl;
        }

        [JsonProperty("artists", Required = Required.Always)]
        [SwaggerSchema("The artists who collaborated on making the track")]
        public IEnumerable<string> Artists { get; set; } = null!;

        [JsonProperty("popularity", Required = Required.Always)]
        [SwaggerSchema("The popularity rank of the song")]
        public int Popularity { get; set; }

        [JsonProperty("preview", Required = Required.Always)]
        [SwaggerSchema("A link to a preview of the track")]
        public string Preview { get; set; } = null!;
    }
}