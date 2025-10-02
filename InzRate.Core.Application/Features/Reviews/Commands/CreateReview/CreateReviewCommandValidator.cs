using FluentValidation;

namespace InzRate.Core.Application.Features.Reviews.Commands.CreateReview;

public class CreateReviewCommandValidator : AbstractValidator<CreateReviewCommand>
{
    public CreateReviewCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required.");

        RuleFor(x => x.MovieId)
            .NotEmpty().WithMessage("Movie ID is required.");

        RuleFor(x => x.Stars)
            .InclusiveBetween(1, 5).WithMessage("Stars must be between 1 and 5.");

        RuleFor(x => x.Text)
            .NotEmpty().WithMessage("Review text is required.")
            .MaximumLength(1000).WithMessage("Review text cannot exceed 1000 characters.");
    }
}