using InzRate.Core.Application.Contracts.Identity;
using InzRate.Core.Infrastructure.Persistence;
using InzRate.Core.Infrastructure.Persistence.Entities;
using Microsoft.AspNetCore.Identity;

namespace InzRate.Core.Infrastructure.Services;

public class AuthService(AppDbContext appDbContext, IPasswordHasher<AppUser> passwordHasher) : IAuthService
{
    public async Task RegisterNewUserAuthenticationDetailsAsync(Guid userId, string password)
    {
        var appUser = await appDbContext.Users.FindAsync(userId);
        if (appUser is null) throw new InvalidOperationException($"User with ID '{userId}' not found. Cannot set up authentication.");

        appUser.PasswordHash = passwordHasher.HashPassword(appUser, password);
        appUser.EmailConfirmed = false;
        appUser.TwoFactorEnabled = false;
    }
}