using InzRate.Core.Application.Contracts.Persistence;
using InzRate.Core.Domain.Entities;
using InzRate.Core.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace InzRate.Core.Infrastructure.Persistence.Repositories;

public class UserRepository(AppDbContext context) : IUserRepository
{
    public async Task<User?> CreateAsync(string username, string email)
    {
        var user = User.Create(username, email);
        var appUser = AppUser.FromDomain(user);
        await context.Users.AddAsync(appUser);
        return user;
    }

    public async Task<bool> IsEmailUnique(string email)
    {
        return !await context.Users.AnyAsync(u => u.Email == email);
    }

    public async Task<bool> IsUsernameUnique(string username)
    {
        return !await context.Users.AnyAsync(u => u.UserName == username);
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
         // We use FindAsync so if the entity is already tracked it will be returned instead of retrieving it again from the database
        var appUser = await context.Users.FindAsync(id);
        return appUser is null ? null : AppUser.ToDomain(appUser);
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        if (string.IsNullOrWhiteSpace(username)) return null;

        var appUser = await context.Users.FirstOrDefaultAsync(u => u.UserName!.ToLower() == username.ToLower());
        return appUser is null ? null : AppUser.ToDomain(appUser);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email)) return null;

        var appUser = await context.Users.FirstOrDefaultAsync(u => u.Email!.ToLower() == email.ToLower());
        return appUser is null ? null : AppUser.ToDomain(appUser);
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await context.Users.Select(e => AppUser.ToDomain(e)).ToListAsync();
    }

    public async Task AddAsync(User user)
    {
        var appUser = AppUser.FromDomain(user);
        await context.Users.AddAsync(appUser);
    }

    public async Task<User?> Update(User user)
    {
        var existingUser = await context.Users.FindAsync(user.Id);
        if (existingUser is null) return null;

        var domainUser = AppUser.ToDomain(existingUser).Update(user.Username, user.Email);
        existingUser.UserName = domainUser.Username;
        existingUser.Email = domainUser.Email;
        // Update other properties ... 
        return domainUser;
    }

    public void Delete(User user)
    {
        var appUser = AppUser.FromDomain(user);
        context.Users.Remove(appUser);
    }
}