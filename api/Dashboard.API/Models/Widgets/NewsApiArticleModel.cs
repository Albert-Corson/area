using NewsAPI.Models;
using Newtonsoft.Json;

namespace Dashboard.API.Models.Widgets
{
    public class NewsApiArticleModel : WidgetCallResponseItemModel
    {
        public NewsApiArticleModel()
        {}

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
        public string? Source { get; set; }

        [JsonProperty("author")]
        public string? Author { get; set; }

        [JsonProperty("description")]
        public string? Description { get; set; }

        [JsonProperty("published_at")]
        public long? PublishedAt { get; set; }
    }
}
