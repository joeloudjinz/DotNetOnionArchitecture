using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using InzRate.Core.Domain.Entities;
using InzRate.Core.Domain.Aggregates;
using InzRate.Core.Infrastructure.Persistence.Configurations;
using InzRate.Core.Infrastructure.Persistence.Entities;
using Microsoft.AspNetCore.Identity;

namespace InzRate.Core.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<AppUser, AppRole, Guid>(options)
{
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


// TODO Implement Role feature
public class AppRole: IdentityRole<Guid>
{
}