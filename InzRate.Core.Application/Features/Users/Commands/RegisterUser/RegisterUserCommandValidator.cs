using FluentValidation;

namespace InzRate.Core.Application.Features.Users.Commands.RegisterUser;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required.")
            .MinimumLength(3).WithMessage("Username must be at least 3 characters long.")
            .MaximumLength(50).WithMessage("Username cannot exceed 50 characters.")
            .Matches("^[a-zA-Z0-9_]*$").WithMessage("Username can only contain letters, numbers, and underscores.");
    }
}