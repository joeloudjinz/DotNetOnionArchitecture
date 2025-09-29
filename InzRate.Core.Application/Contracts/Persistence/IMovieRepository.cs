using InzRate.Core.Domain.Entities;

namespace InzRate.Core.Application.Contracts.Persistence;

public interface IMovieRepository
{
    Task<Movie?> GetByIdAsync(Guid id);
    Task<IEnumerable<Movie>> GetAllAsync();
    Task<Movie?> GetByTitleAsync(string title);
    Task<IEnumerable<Movie>> GetByReleaseYearAsync(int releaseYear);
    Task<IEnumerable<Movie>> GetWithAverageRatingAsync();
    Task<double?> GetAverageRatingAsync(Guid movieId);
    Task AddAsync(Movie movie);
    void Update(Movie movie);
    void Delete(Movie movie);
}