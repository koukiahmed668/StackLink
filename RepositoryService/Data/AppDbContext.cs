using Microsoft.EntityFrameworkCore;
using RepositoryService.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace RepositoryService.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Repository> Repositories { get; set; }
    public DbSet<Contributor> Contributors { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Contributor>()
            .HasOne<Repository>()
            .WithMany()
            .HasForeignKey(c => c.RepositoryId);

        base.OnModelCreating(modelBuilder);
    }
}
