using RepositoryService.Data;
using RepositoryService.Models;
using Microsoft.EntityFrameworkCore;

namespace RepositoryService.Services;

public class RepoService
{
    private readonly AppDbContext _dbContext;

    public RepoService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Repository>> GetAllRepositoriesAsync()
    {
        return await _dbContext.Repositories.ToListAsync();
    }

    public async Task AddRepositoryAsync(Repository repository)
    {
        _dbContext.Repositories.Add(repository);
        await _dbContext.SaveChangesAsync();
    }
}
