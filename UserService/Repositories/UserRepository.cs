using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.Models;

namespace UserService.Repositories;

public class UserRepository
{
    private readonly AppDbContext _dbContext;

    public UserRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public User? GetUserByEmail(string email) => _dbContext.Users.FirstOrDefault(u => u.Email == email);

    public void AddUser(User user) => _dbContext.Users.Add(user);
}
