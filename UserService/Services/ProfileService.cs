using UserService.Models;
using UserService.Data;

namespace UserService.Services;

public class ProfileService
{
    private readonly AppDbContext _dbContext;

    public ProfileService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Profile? GetProfile(Guid userId) =>
        _dbContext.Profiles.FirstOrDefault(p => p.UserId == userId);

    public void CreateProfile(Profile profile)
    {
        _dbContext.Profiles.Add(profile);
        _dbContext.SaveChanges();
    }

    public void AddBookmark(Guid userId, string repoName, string repoUrl)
    {
        // Check if the bookmark already exists
        var existingBookmark = _dbContext.Bookmarks
            .FirstOrDefault(b => b.UserId == userId && b.RepositoryUrl == repoUrl);

        if (existingBookmark != null) return; // Prevent duplicates

        // Create new bookmark
        var newBookmark = new Bookmark
        {
            Id = Guid.NewGuid(),
            RepositoryName = repoName,
            RepositoryUrl = repoUrl,
            UserId = userId
        };

        _dbContext.Bookmarks.Add(newBookmark);
        _dbContext.SaveChanges();
    }

    public List<Bookmark> GetBookmarks(Guid userId) =>
        _dbContext.Bookmarks.Where(b => b.UserId == userId).ToList();
}
