using InzRate.Core.Application.Contracts.Persistence;
using InzRate.Core.Infrastructure.Persistence.Repositories;

namespace InzRate.Core.Infrastructure.Persistence;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    public async Task<int> SaveChangesAsync()
    {
        return await context.SaveChangesAsync();
    }

    public void Dispose()
    {
        context?.Dispose();
    }
}