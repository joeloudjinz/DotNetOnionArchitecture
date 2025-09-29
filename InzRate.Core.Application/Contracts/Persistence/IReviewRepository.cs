using InzRate.Core.Domain.Aggregates;

namespace InzRate.Core.Application.Contracts.Persistence;

public interface IReviewRepository
{
    Task<Review?> GetByIdAsync(Guid id);
    Task<IEnumerable<Review>> GetAllAsync();
    Task<IEnumerable<Review>> GetByMovieIdAsync(Guid movieId);
    Task<IEnumerable<Review>> GetByUserIdAsync(Guid userId);
    Task<Review?> GetByUserAndMovieIdAsync(Guid userId, Guid movieId);
    Task<bool> HasUserRatedMovieAsync(Guid userId, Guid movieId);
    Task AddAsync(Review review);
    void Update(Review review);
    void Delete(Review review);
}