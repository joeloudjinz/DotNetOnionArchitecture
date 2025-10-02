using InzRate.Core.Domain.Entities;

namespace InzRate.Core.Application.Contracts.Persistence;

public interface IUserRepository
{
    Task<User?> CreateAsync(string username, string email);
    Task<bool> IsEmailUnique(string email);
    Task<bool> IsUsernameUnique(string username);
    Task<User?> GetByIdAsync(Guid id);
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByEmailAsync(string username);
    Task<IEnumerable<User>> GetAllAsync();
    Task AddAsync(User user);
    Task<User?> Update(User user);
    void Delete(User user);
}