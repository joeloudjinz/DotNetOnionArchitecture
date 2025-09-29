using InzRate.Core.Application.Contracts.Persistence;
using InzRate.Core.Infrastructure.Persistence.Repositories;

namespace InzRate.Core.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    
    private IReviewRepository? _reviewRepository;
    private IMovieRepository? _movieRepository;
    private IUserRepository? _userRepository;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public IReviewRepository ReviewRepository => 
        _reviewRepository ??= new ReviewRepository(_context);

    public IMovieRepository MovieRepository => 
        _movieRepository ??= new MovieRepository(_context);

    public IUserRepository UserRepository => 
        _userRepository ??= new UserRepository(_context);

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context?.Dispose();
    }
}