using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SpotifyAPI.Web;
using Swashbuckle.AspNetCore.Annotations;

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

        [JsonProperty("followers", Required = Required.Always)]
        [SwaggerSchema("The amount of followers the artist has")]
        public int Followers { get; set; }

        [JsonProperty("genres", Required = Required.Always)]
        [SwaggerSchema("The genres of music the artist makes")]
        public IEnumerable<string> Genres { get; set; } = null!;

        [JsonProperty("popularity", Required = Required.Always)]
        [SwaggerSchema("The popularity rank of the artist")]
        public int Popularity { get; set; }
    }
}