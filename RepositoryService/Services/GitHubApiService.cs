using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RepositoryService.Models;

namespace RepositoryService.Services;

public class GitHubApiService
{
    private readonly HttpClient _httpClient;

    public GitHubApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;

        // Required User-Agent header for GitHub API
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "StackLink-App");

        // Optional: Add Authorization for higher rate limits
        var githubToken = Environment.GetEnvironmentVariable("GITHUB_PAT");
        if (!string.IsNullOrEmpty(githubToken))
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", githubToken);
        }
    }

    public async Task<List<Repository>> GetTrendingRepositoriesAsync(string? language = null, string timeframe = "today", int minStars = 50)
    {
        // Get local time filters
        var today = DateTime.Now.ToString("yyyy-MM-dd");
        var weekStart = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
        var monthStart = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");

        // Select date filter based on timeframe
        var dateFilter = timeframe switch
        {
            "today" => today,
            "this_week" => weekStart,
            "this_month" => monthStart,
            _ => today
        };

        // Validate language filter (if provided)
        var validLanguages = new List<string>
    {
        "JavaScript", "Python", "Java", "C#", "Ruby", "TypeScript", "PHP", "Go", "C++", "Swift"
    };

        var languageFilter = string.IsNullOrEmpty(language) || !validLanguages.Contains(language) ? "" : $"language:{language}+";

        // Build the API URL with the dynamic minStars value
        var apiUrl = $"https://api.github.com/search/repositories?q={languageFilter}stars:>={minStars}+created:>{dateFilter}&sort=stars&order=desc";

        // Make the request
        var response = await _httpClient.GetAsync(apiUrl);
        response.EnsureSuccessStatusCode(); // Ensures request succeeds; throws otherwise

        var responseContent = await response.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<JObject>(responseContent);

        var repositories = data["items"]?.ToObject<List<JObject>>() ?? new List<JObject>();

        return repositories.Select(repo => new Repository
        {
            Id = Guid.NewGuid(),
            Name = repo["name"]?.ToString() ?? string.Empty,
            Owner = repo["owner"]?["login"]?.ToString() ?? string.Empty,
            Url = repo["html_url"]?.ToString() ?? string.Empty,
            Stars = repo["stargazers_count"]?.Value<int>() ?? 0,
            Language = repo["language"]?.ToString(),
            Description = repo["description"]?.ToString()
        }).ToList();
    }

    public async Task<List<Contributor>> GetContributorsAsync(string owner, string repoName)
    {
        var response = await _httpClient.GetAsync($"https://api.github.com/repos/{owner}/{repoName}/contributors");
        response.EnsureSuccessStatusCode(); // Ensures request succeeds; throws otherwise

        var responseContent = await response.Content.ReadAsStringAsync();
        var data = JsonConvert.DeserializeObject<JArray>(responseContent);

        return data?.Select(contributor => new Contributor
        {
            Id = Guid.NewGuid(),
            Username = contributor["login"]?.ToString() ?? string.Empty,
            ProfileUrl = contributor["html_url"]?.ToString() ?? string.Empty,
            RepositoryId = Guid.Empty // Will be set when adding to the database
        }).ToList() ?? new List<Contributor>();
    }
}
