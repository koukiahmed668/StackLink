using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using NewsService.Models;
using System.Collections.Generic;

namespace NewsService.Services
{
    public class News
    {
        private readonly HttpClient _httpClient;

        public News(HttpClient httpClient)
        {
            _httpClient = httpClient;

            // Required User-Agent header for the API
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "StackLink-App");

            // Optional: Add Authorization for higher rate limits (if necessary)
            var newsApiToken = Environment.GetEnvironmentVariable("NEWS_API_KEY");
            if (string.IsNullOrEmpty(newsApiToken))
            {
                throw new InvalidOperationException("News API key is not set.");
            }
            else
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", newsApiToken);
            }
        }

        public async Task<List<NewsArticle>> GetNewsByQueryAsync(string query)
        {
            // Set up the API URL based on the query
            var apiUrl = $"https://newsapi.org/v2/everything?q={query}&sortBy=publishedAt&apiKey={Environment.GetEnvironmentVariable("NEWS_API_KEY")}";

            var response = await _httpClient.GetAsync(apiUrl);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();

            // Log or inspect the raw response content for debugging
            Console.WriteLine(responseContent);

            var newsData = JsonConvert.DeserializeObject<NewsApiResponse>(responseContent);

            return newsData?.Articles ?? new List<NewsArticle>();
        }

    }
}
