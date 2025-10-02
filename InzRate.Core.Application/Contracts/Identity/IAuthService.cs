using InzRate.Core.Domain.Entities;

namespace InzRate.Core.Application.Contracts.Identity;

public interface IAuthService
{
    string HashUserPasswordAsync(User user, string password);
    Task RegisterNewUserAuthenticationDetailsAsync(Guid userId, string password);
    Task HashUserPasswordAsync(Guid userId, string password);
    bool VerifyUserPasswordAsync(User user, string password);
    Task<bool> VerifyUserPasswordAsync(Guid userId, string password);
}