using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SpotifyAPI.Web;

namespace Dashboard.API.Models.Widgets
{
    public class SpotifyTrackModel : WidgetCallResponseItemModel
    {
        public SpotifyTrackModel()
        {}

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

        [JsonProperty("artists")]
        public IEnumerable<string>? Artists { get; set; }

        [JsonProperty("popularity")]
        public int? Popularity { get; set; }

        [JsonProperty("preview")]
        public string? Preview { get; set; }
    }
}
