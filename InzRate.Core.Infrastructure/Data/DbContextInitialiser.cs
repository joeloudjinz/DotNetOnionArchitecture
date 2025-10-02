using InzRate.Core.Infrastructure.Persistence;
using InzRate.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InzRate.Core.Infrastructure.Data;

public class DbContextInitialiser(AppDbContext context)
{
    public async Task InitialiseAsync()
    {
        if (context.Database.GetPendingMigrations().Any())
        {
            await context.Database.MigrateAsync();
        }
    }

    public async Task SeedAsync()
    {
        // Check if movies already exist to avoid duplicates
        if (await context.Movies.AnyAsync())
        {
            return; // Data already seeded
        }

        // Create initial movie data
        var movies = new List<Movie>
        {
            new Movie
            {
                Id = Guid.NewGuid(),
                Title = "The Shawshank Redemption",
                ReleaseYear = 1994
            },
            new Movie
            {
                Id = Guid.NewGuid(),
                Title = "The Godfather",
                ReleaseYear = 1972
            },
            new Movie
            {
                Id = Guid.NewGuid(),
                Title = "The Dark Knight",
                ReleaseYear = 2008
            },
            new Movie
            {
                Id = Guid.NewGuid(),
                Title = "Pulp Fiction",
                ReleaseYear = 1994
            },
            new Movie
            {
                Id = Guid.NewGuid(),
                Title = "Forrest Gump",
                ReleaseYear = 1994
            },
            new Movie
            {
                Id = Guid.NewGuid(),
                Title = "Inception",
                ReleaseYear = 2010
            },
            new Movie
            {
                Id = Guid.NewGuid(),
                Title = "The Matrix",
                ReleaseYear = 1999
            },
            new Movie
            {
                Id = Guid.NewGuid(),
                Title = "Goodfellas",
                ReleaseYear = 1990
            },
            new Movie
            {
                Id = Guid.NewGuid(),
                Title = "Parasite",
                ReleaseYear = 2019
            },
            new Movie
            {
                Id = Guid.NewGuid(),
                Title = "Spirited Away",
                ReleaseYear = 2001
            }
        };

        // Add movies to the context
        foreach (var movie in movies)
        {
            _ = context.Movies.Add(movie);
        }

        // Save changes to the database
        await context.SaveChangesAsync();
    }
}