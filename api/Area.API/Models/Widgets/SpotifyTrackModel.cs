using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SpotifyAPI.Web;
using Swashbuckle.AspNetCore.Annotations;

namespace Area.API.Models.Widgets
{
    [SwaggerSchema(@"## This is the response model for the following widgets of the Spotify service:
- Favorite tracks
- History")]
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
            else if (track.ExternalUrls.Any())
                Link = track.ExternalUrls.First().Value;

            Artists = track.Artists.Select(artist => artist.Name);
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

        [JsonProperty("preview", Required = Required.Always)]
        [SwaggerSchema("A link to a preview of the track")]
        public string Preview { get; set; } = null!;
    }
}