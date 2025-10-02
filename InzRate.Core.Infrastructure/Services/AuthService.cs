using InzRate.Core.Application.Contracts.Identity;
using InzRate.Core.Application.Contracts.Persistence;
using InzRate.Core.Domain.Entities;
using InzRate.Core.Infrastructure.Persistence.Entities;
using Microsoft.AspNetCore.Identity;

namespace InzRate.Core.Infrastructure.Services;

public class AuthService(IUserRepository userRepository, UserManager<AppUser> userManager) : IAuthService
{
    // Note that the first argument to PasswordHasher.HashPassword() is a user instance, which can be null.
    // It's used to determine hashing compatibility options, but for new hashes, it's safe to set it to `null`.
    
    public string HashUserPasswordAsync(User user, string password)
    {
        var appUser = AppUser.FromDomain(user);
        appUser.PasswordHash = userManager.PasswordHasher.HashPassword(appUser, password);
        return appUser.PasswordHash;
    }

    public async Task RegisterNewUserAuthenticationDetailsAsync(Guid userId, string password)
    {
        var user = await userRepository.GetByIdAsync(userId);
        if (user is null) throw new Exception("User not found");

        var appUser = AppUser.FromDomain(user);
        appUser.PasswordHash = userManager.PasswordHasher.HashPassword(appUser, password);
        appUser.EmailConfirmed = false;
        appUser.TwoFactorEnabled = false;
        // Set application properties for newly registered user ... 
        await userRepository.Update(user);
    }

    public async Task HashUserPasswordAsync(Guid userId, string password)
    {
        var user = await userRepository.GetByIdAsync(userId);
        if (user is null) throw new Exception("User not found");

        var appUser = AppUser.FromDomain(user);
        appUser.PasswordHash = userManager.PasswordHasher.HashPassword(appUser, password);
        await userRepository.Update(user);
    }

    public bool VerifyUserPasswordAsync(User user, string password)
    {
        var appUser = AppUser.FromDomain(user);
        var result = userManager.PasswordHasher.VerifyHashedPassword(appUser, appUser.PasswordHash!, password);
        return result == PasswordVerificationResult.Success;
    }

    public async Task<bool> VerifyUserPasswordAsync(Guid userId, string password)
    {
        var user = await userRepository.GetByIdAsync(userId);
        if (user is null) throw new Exception("User not found");

        var appUser = AppUser.FromDomain(user);
        var result = userManager.PasswordHasher.VerifyHashedPassword(appUser, appUser.PasswordHash!, password);
        return result == PasswordVerificationResult.Success;
    }
}