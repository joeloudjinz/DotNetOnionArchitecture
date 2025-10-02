using InzRate.Core.Application.Contracts.Identity;
using InzRate.Core.Application.Contracts.Persistence;
using InzRate.Core.Infrastructure.Persistence;
using InzRate.Core.Infrastructure.Persistence.Entities;
using InzRate.Core.Infrastructure.Persistence.Repositories;
using InzRate.Core.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InzRate.Core.Infrastructure.Extensions;

public static class ServicesCollectionExtensions
{
    public static void RegisterPersistenceServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options => { options.UseSqlite(); });
    }

    public static void RegisterIdentityServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Use AddDefaultTokenProviders() when adding JWT support
        services.AddIdentityCore<AppUser>()
            .AddRoles<AppRole>()
            .AddEntityFrameworkStores<AppDbContext>();
    }

    public static void RegisterCoreServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Core
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IMovieRepository, MovieRepository>();
        services.AddScoped<IReviewRepository, ReviewRepository>();

        // Services
        services.AddSingleton<IAuthService, AuthService>();
    }

    public static void RegisterCommandsAndQueries(this IServiceCollection services, IConfiguration configuration)
    {
        // Add MediatR registration
        services.AddMediatR(cfg =>
        {
            // Scan the assembly containing the commands/handlers for automatic registration
            cfg.RegisterServicesFromAssembly(typeof(Application.Root).Assembly);

            // TODO Register behaviors here
        });
    }
}