using Microsoft.EntityFrameworkCore;
using InzRate.Core.Application.Contracts.Persistence;
using InzRate.Core.Domain.Aggregates;

namespace InzRate.Core.Infrastructure.Persistence.Repositories;

public class ReviewRepository(AppDbContext context) : IReviewRepository
{
    public async Task<Review?> GetByIdAsync(Guid id)
    {
        return await context.Reviews
            .Include(r => r.Rating)  // Include the Rating value object if needed
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<IEnumerable<Review>> GetAllAsync()
    {
        return await context.Reviews
            .Include(r => r.Rating)
            .ToListAsync();
    }

    public async Task<IEnumerable<Review>> GetByMovieIdAsync(Guid movieId)
    {
        return await context.Reviews
            .Include(r => r.Rating)
            .Where(r => r.MovieId == movieId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Review>> GetByUserIdAsync(Guid userId)
    {
        return await context.Reviews
            .Include(r => r.Rating)
            .Where(r => r.UserId == userId)
            .ToListAsync();
    }

    public async Task<Review?> GetByUserAndMovieIdAsync(Guid userId, Guid movieId)
    {
        return await context.Reviews
            .Include(r => r.Rating)
            .FirstOrDefaultAsync(r => r.UserId == userId && r.MovieId == movieId);
    }

    public async Task<bool> HasUserRatedMovieAsync(Guid userId, Guid movieId)
    {
        return await context.Reviews
            .AnyAsync(r => r.UserId == userId && r.MovieId == movieId);
    }

    public async Task AddAsync(Review review)
    {
        await context.Reviews.AddAsync(review);
    }

    public void Update(Review review)
    {
        context.Reviews.Update(review);
    }

    public void Delete(Review review)
    {
        context.Reviews.Remove(review);
    }
}