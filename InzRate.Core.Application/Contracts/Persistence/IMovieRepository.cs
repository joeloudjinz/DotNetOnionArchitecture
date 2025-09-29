using InzRate.Core.Domain.Entities;

namespace InzRate.Core.Application.Contracts.Persistence;

public interface IMovieRepository
{
    Task<Movie?> GetByIdAsync(Guid id);
    Task<IEnumerable<Movie>> GetAllAsync();
    Task<Movie?> GetByTitleAsync(string title);
    Task AddAsync(Movie movie);
    void Update(Movie movie);
    void Delete(Movie movie);
}