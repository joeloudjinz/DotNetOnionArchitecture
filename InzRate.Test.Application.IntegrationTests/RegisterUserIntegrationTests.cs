using InzRate.Core.Application.Contracts.Persistence;
using InzRate.Core.Application.Features.Users.Commands.RegisterUser;
using InzRate.Core.Infrastructure.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace InzRate.Test.Application.IntegrationTests;

public class RegisterUserIntegrationTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    [Fact]
    public async Task RegisterUserCommandHandler_HappyPath_CreatesUserSuccessfully()
    {
        // Arrange
        var scope = factory.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        var command = new RegisterUserCommand(
            Username: "testuser",
            Email: "test@example.com",
            Password: "TestPassword123!"
        );

        // Act
        var userId = await mediator.Send(command);

        // Assert
        Assert.NotEqual(Guid.Empty, userId);

        // Get the user from the database using the userId and assert that
        var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
        var user = await userRepository.GetByIdAsync(userId);
        
        // Assert that user record is not null
        Assert.NotNull(user);
        Assert.Equal(userId, user.Id);
        
        // Assert that properties in command are exactly the same in the user record
        Assert.Equal("testuser", user.Username);
        Assert.Equal("test@example.com", user.Email);
        
        // Get the AppUser from the database to check password hash and ASP.NET properties
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var appUser = await dbContext.Users.FindAsync(userId);
        
        // Assert that the password is a hash string (not empty or null)
        Assert.NotNull(appUser?.PasswordHash);
        Assert.NotEmpty(appUser.PasswordHash);
        Assert.StartsWith("AQAAAAIAAYagAAAA", appUser.PasswordHash); // ASP.NET Identity password hashes start with this prefix
        
        // Assert that other properties handled by ASP.NET Identity are populated correctly
        Assert.False(appUser.EmailConfirmed); // As set in AuthService.RegisterNewUserAuthenticationDetailsAsync
        Assert.False(appUser.TwoFactorEnabled); // As set in AuthService.RegisterNewUserAuthenticationDetailsAsync
        Assert.NotNull(appUser.UserName);
        Assert.NotNull(appUser.NormalizedUserName);
        Assert.NotNull(appUser.Email);
        Assert.NotNull(appUser.NormalizedEmail);
        
        // Additional checks for the user entity properties
        Assert.Equal("testuser", appUser.UserName);
        Assert.Equal("TESTUSER", appUser.NormalizedUserName);
        Assert.Equal("test@example.com", appUser.Email);
        Assert.Equal("TEST@EXAMPLE.COM", appUser.NormalizedEmail);
    }
}