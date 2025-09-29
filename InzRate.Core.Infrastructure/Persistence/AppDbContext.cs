using Microsoft.EntityFrameworkCore;
using InzRate.Core.Domain.Entities;
using InzRate.Core.Domain.Aggregates;
using InzRate.Core.Infrastructure.Persistence.Configurations;

namespace InzRate.Core.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Movie> Movies { get; set; }
    public DbSet<Review> Reviews { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Apply configurations using Fluent API
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new MovieConfiguration());
        modelBuilder.ApplyConfiguration(new ReviewConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}