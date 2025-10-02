using Microsoft.EntityFrameworkCore;
using InzRate.Core.Application.Contracts.Persistence;
using InzRate.Core.Domain.Entities;

namespace InzRate.Core.Infrastructure.Persistence.Repositories;

public class MovieRepository(AppDbContext context) : IMovieRepository
{
    public async Task<Movie?> GetByIdAsync(Guid id)
    {
        return await context.Movies
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<IEnumerable<Movie>> GetAllAsync()
    {
        return await context.Movies
            .ToListAsync();
    }

    public async Task<Movie?> GetByTitleAsync(string title)
    {
        return await context.Movies
            .FirstOrDefaultAsync(m => m.Title.ToLower() == title.ToLower());
    }

    public async Task<IEnumerable<Movie>> GetByReleaseYearAsync(int releaseYear)
    {
        return await context.Movies
            .Where(m => m.ReleaseYear == releaseYear)
            .ToListAsync();
    }

    public async Task<IEnumerable<Movie>> GetWithAverageRatingAsync()
    {
        // This requires a more complex query to calculate average ratings
        // For now, I'll return all movies and average calculation can be done in the application layer
        return await context.Movies
            .ToListAsync();
    }

    public async Task<double?> GetAverageRatingAsync(Guid movieId)
    {
        // Get all reviews for this movie and calculate average
        var reviews = await context.Reviews
            .Where(r => r.MovieId == movieId)
            .ToListAsync();

        if (reviews.Any())
        {
            return reviews.Average(r => r.Rating.Value);
        }

        return null; // No reviews for this movie
    }

    public async Task AddAsync(Movie movie)
    {
        await context.Movies.AddAsync(movie);
    }

    public void Update(Movie movie)
    {
        context.Movies.Update(movie);
    }

    public void Delete(Movie movie)
    {
        context.Movies.Remove(movie);
    }
}