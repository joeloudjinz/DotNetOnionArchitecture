using MediatR;
using InzRate.Core.Application.Contracts.Persistence;
using InzRate.Core.Domain.Entities;
using InzRate.Core.Application.Features.Users.Commands.RegisterUser;

namespace InzRate.Core.Application.Features.Users.Commands.RegisterUser;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;

    public RegisterUserCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        // Check if a user with the same username already exists
        var existingUser = await _unitOfWork.UserRepository.GetByUsernameAsync(request.Username);
        if (existingUser != null)
        {
            throw new InvalidOperationException($"A user with username '{request.Username}' already exists.");
        }

        // Create a new User entity
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = request.Username
        };

        // Add the user to the repository
        await _unitOfWork.UserRepository.AddAsync(user);

        // Commit the transaction
        await _unitOfWork.SaveChangesAsync();

        // Return the ID of the created user
        return user.Id;
    }
}