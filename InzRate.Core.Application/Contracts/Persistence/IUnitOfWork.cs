namespace InzRate.Core.Application.Contracts.Persistence;

public interface IUnitOfWork : IDisposable
{
    Task<int> SaveChangesAsync();
}