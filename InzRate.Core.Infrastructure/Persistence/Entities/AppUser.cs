using InzRate.Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace InzRate.Core.Infrastructure.Persistence.Entities;

public class AppUser : IdentityUser<Guid>
{
    public static AppUser FromDomain(User user)
    {
        return new AppUser
        {
            Id = user.Id,
            Email = user.Email,
            NormalizedEmail = user.Email.ToUpper(),
            UserName = user.Username,
            NormalizedUserName = user.Username.ToUpper()
        };
    }

    public static User ToDomain(AppUser user)
    {
        return User.Create(user.Id, user.UserName!, user.Email!);
    }
}