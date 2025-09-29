using InzRate.Core.Domain.Aggregates;

namespace InzRate.Core.Application.Contracts.Persistence;

public interface IReviewRepository
{
    Task<Review?> GetByIdAsync(Guid id);
    Task<IEnumerable<Review>> GetAllAsync();
    Task AddAsync(Review review);
    void Update(Review review);
    void Delete(Review review);
}