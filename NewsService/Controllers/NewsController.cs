using Microsoft.AspNetCore.Mvc;
using NewsService.Models;
using NewsService.Services;

namespace NewsService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly News _newsService;

        public NewsController(News newsService)
        {
            _newsService = newsService;
        }

        [HttpGet("search")]
        public async Task<IActionResult> GetNewsByQuery([FromQuery] string query)
        {
            var newsResults = await _newsService.GetNewsByQueryAsync(query);
            return Ok(newsResults);
        }

    }
}
