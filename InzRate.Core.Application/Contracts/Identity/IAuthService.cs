using InzRate.Core.Domain.Entities;

namespace InzRate.Core.Application.Contracts.Identity;

public interface IAuthService
{
    Task RegisterNewUserAuthenticationDetailsAsync(Guid userId, string password);
}