using MediatR;

namespace InzRate.Core.Application.Features.Users.Commands.RegisterUser;

public record RegisterUserCommand(
    string Username
) : IRequest<Guid>;