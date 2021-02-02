using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SpotifyAPI.Web;

namespace Area.API.Models.Widgets
{
    public class SpotifyArtistModel : WidgetCallResponseItemModel
    {
        public SpotifyArtistModel()
        { }

        public SpotifyArtistModel(FullArtist artist)
        {
            Image = artist.Images.FirstOrDefault()?.Url;
            Header = artist.Name;
            if (artist.ExternalUrls.TryGetValue("spotify", out var link))
                Link = link;
            else if (artist.ExternalUrls.Count > 0)
                Link = artist.ExternalUrls.FirstOrDefault().Value;

            Followers = artist.Followers.Total;
            Genres = artist.Genres;
            Popularity = artist.Popularity;
        }

        [JsonProperty("followers")]
        public int Followers { get; set; }

        [JsonProperty("genres")]
        public IEnumerable<string> Genres { get; set; } = null!;

        [JsonProperty("popularity")]
        public int Popularity { get; set; }
    }
}