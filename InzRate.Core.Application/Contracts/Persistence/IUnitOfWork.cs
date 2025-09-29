using InzRate.Core.Application.Contracts.Persistence;

namespace InzRate.Core.Application.Contracts.Persistence;

public interface IUnitOfWork : IDisposable
{
    IReviewRepository ReviewRepository { get; }
    IUserRepository UserRepository { get; }
    IMovieRepository MovieRepository { get; }
    
    Task<int> SaveChangesAsync();
}