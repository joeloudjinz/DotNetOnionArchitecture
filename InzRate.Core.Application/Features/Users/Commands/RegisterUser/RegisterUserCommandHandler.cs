using InzRate.Core.Application.Contracts.Identity;
using InzRate.Core.Application.Contracts.Persistence;
using MediatR;

namespace InzRate.Core.Application.Features.Users.Commands.RegisterUser;

public class RegisterUserCommandHandler(IUnitOfWork unitOfWork, IUserRepository userRepository, IAuthService authService) : IRequestHandler<RegisterUserCommand, Guid>
{
    public async Task<Guid> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        if (!await userRepository.IsEmailUnique(request.Email)) throw new InvalidOperationException("Email already taken.");
        if (!await userRepository.IsUsernameUnique(request.Username)) throw new InvalidOperationException("Username already taken");

        var user = await userRepository.CreateAsync(request.Username, request.Email);
        if (user is null) throw new Exception("Failed to create user");

        await authService.RegisterNewUserAuthenticationDetailsAsync(user.Id, request.Password);

        // Attach role
        // Generate tokens

        await unitOfWork.SaveChangesAsync();
        return user.Id;
    }
}