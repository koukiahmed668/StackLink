namespace NewsService.Models
{
    public class NewsArticle
    {
        public string Author { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string UrlToImage { get; set; }
        public DateTime PublishedAt { get; set; }
        public Source Source { get; set; } // Updated to match the object structure
    }

    public class Source
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class NewsApiResponse
    {
        public string Status { get; set; }
        public int TotalResults { get; set; }
        public List<NewsArticle> Articles { get; set; }
    }
}
