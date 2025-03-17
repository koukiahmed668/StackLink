using Microsoft.AspNetCore.Mvc;
using RepositoryService.Models;
using RepositoryService.Services;

namespace RepositoryService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RepositoryController : ControllerBase
{
    private readonly RepoService _repositoryService;
    private readonly GitHubApiService _gitHubApiService;

    public RepositoryController(RepoService repositoryService, GitHubApiService gitHubApiService)
    {
        _repositoryService = repositoryService;
        _gitHubApiService = gitHubApiService;
    }

    [HttpGet("trending")]
    public async Task<IActionResult> GetTrendingRepositories([FromQuery] string? language = null, [FromQuery] string timeframe = "today", [FromQuery] int minStars = 50)
    {
        // Call service method to fetch trending repositories with filter options
        var trendingRepos = await _gitHubApiService.GetTrendingRepositoriesAsync(language, timeframe, minStars);
        return Ok(trendingRepos);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllRepositories()
    {
        var repositories = await _repositoryService.GetAllRepositoriesAsync();
        return Ok(repositories);
    }

    [HttpPost]
    public async Task<IActionResult> AddRepository([FromBody] Repository repository)
    {
        await _repositoryService.AddRepositoryAsync(repository);
        return Ok("Repository added successfully.");
    }
}
