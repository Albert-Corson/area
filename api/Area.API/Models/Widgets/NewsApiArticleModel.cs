using NewsAPI.Models;
using Newtonsoft.Json;

namespace Area.API.Models.Widgets
{
    public class NewsApiArticleModel : WidgetCallResponseItemModel
    {
        public NewsApiArticleModel()
        { }

        public NewsApiArticleModel(Article article)
        {
            Header = article.Title;
            Content = article.Content;
            Link = article.Url;
            Image = article.UrlToImage;

            Source = article.Source.Name;
            Author = article.Author;
            Description = article.Description;
            PublishedAt = article.PublishedAt?.Ticks;
        }

        [JsonProperty("source")]
        public string Source { get; set; } = null!;

        [JsonProperty("author")]
        public string Author { get; set; } = null!;

        [JsonProperty("description")]
        public string Description { get; set; } = null!;

        [JsonProperty("published_at")]
        public long? PublishedAt { get; set; }
    }
}