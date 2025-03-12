using Microsoft.AspNetCore.Mvc;
using UserService.Services;
using UserService.DTOs;
namespace UserService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProfileController : ControllerBase
{
    private readonly ProfileService _profileService;

    public ProfileController(ProfileService profileService)
    {
        _profileService = profileService;
    }

    [HttpPost("{userId}/bookmarks")]
    public IActionResult AddBookmark(Guid userId, [FromBody] BookmarkDto bookmarkDto)
    {
        _profileService.AddBookmark(userId, bookmarkDto.RepositoryName, bookmarkDto.RepositoryUrl);
        return Ok("Bookmark added successfully.");
    }

    [HttpGet("{userId}/bookmarks")]
    public IActionResult GetBookmarks(Guid userId)
    {
        var bookmarks = _profileService.GetBookmarks(userId);
        return Ok(bookmarks);
    }
}
