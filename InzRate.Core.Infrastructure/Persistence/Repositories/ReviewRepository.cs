using Microsoft.EntityFrameworkCore;
using InzRate.Core.Application.Contracts.Persistence;
using InzRate.Core.Domain.Aggregates;
using InzRate.Core.Infrastructure.Persistence;

namespace InzRate.Core.Infrastructure.Persistence.Repositories;

public class ReviewRepository : IReviewRepository
{
    private readonly AppDbContext _context;

    public ReviewRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Review?> GetByIdAsync(Guid id)
    {
        return await _context.Reviews
            .Include(r => r.Rating)  // Include the Rating value object if needed
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<IEnumerable<Review>> GetAllAsync()
    {
        return await _context.Reviews
            .Include(r => r.Rating)
            .ToListAsync();
    }

    public async Task<IEnumerable<Review>> GetByMovieIdAsync(Guid movieId)
    {
        return await _context.Reviews
            .Include(r => r.Rating)
            .Where(r => r.MovieId == movieId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Review>> GetByUserIdAsync(Guid userId)
    {
        return await _context.Reviews
            .Include(r => r.Rating)
            .Where(r => r.UserId == userId)
            .ToListAsync();
    }

    public async Task<Review?> GetByUserAndMovieIdAsync(Guid userId, Guid movieId)
    {
        return await _context.Reviews
            .Include(r => r.Rating)
            .FirstOrDefaultAsync(r => r.UserId == userId && r.MovieId == movieId);
    }

    public async Task<bool> HasUserRatedMovieAsync(Guid userId, Guid movieId)
    {
        return await _context.Reviews
            .AnyAsync(r => r.UserId == userId && r.MovieId == movieId);
    }

    public async Task AddAsync(Review review)
    {
        await _context.Reviews.AddAsync(review);
    }

    public void Update(Review review)
    {
        _context.Reviews.Update(review);
    }

    public void Delete(Review review)
    {
        _context.Reviews.Remove(review);
    }
}