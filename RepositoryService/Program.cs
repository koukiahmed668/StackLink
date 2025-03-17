using Microsoft.EntityFrameworkCore;
using RepositoryService.Data;
using RepositoryService.Services;

namespace RepositoryService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");
            }

            // Add DbContext and repository
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseMySQL(connectionString));


            // Register HttpClient
            builder.Services.AddHttpClient();

            builder.Services.AddDistributedMemoryCache();

            // Register GitHubService as Scoped
            builder.Services.AddScoped<GitHubApiService>();
            builder.Services.AddScoped<RepoService>();


            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
