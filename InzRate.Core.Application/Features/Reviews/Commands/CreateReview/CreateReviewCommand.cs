using MediatR;

namespace InzRate.Core.Application.Features.Reviews.Commands.CreateReview;

public record CreateReviewCommand(
    Guid UserId,
    Guid MovieId,
    int Stars,
    string Text
) : IRequest<Guid>;