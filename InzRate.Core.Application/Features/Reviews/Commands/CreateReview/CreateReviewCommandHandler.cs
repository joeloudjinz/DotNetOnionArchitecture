using MediatR;
using InzRate.Core.Application.Contracts.Persistence;
using InzRate.Core.Domain.Aggregates;
using InzRate.Core.Domain.ValueObjects;

namespace InzRate.Core.Application.Features.Reviews.Commands.CreateReview;

public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateReviewCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
    {
        // Validate that the movie exists
        var movie = await _unitOfWork.MovieRepository.GetByIdAsync(request.MovieId);
        if (movie == null)
        {
            throw new ArgumentException($"Movie with ID {request.MovieId} does not exist.", nameof(request.MovieId));
        }

        // Validate the user has not already rated this movie
        var hasUserRatedMovie = await _unitOfWork.ReviewRepository.HasUserRatedMovieAsync(request.UserId, request.MovieId);
        if (hasUserRatedMovie)
        {
            throw new InvalidOperationException($"User {request.UserId} has already rated movie {request.MovieId}.");
        }

        // Create the Rating value object
        var rating = new Rating(request.Stars);

        // Create the Review aggregate using its factory method
        var review = Review.Create(request.MovieId, request.UserId, rating, request.Text);

        // Add the new review to the repository
        await _unitOfWork.ReviewRepository.AddAsync(review);

        // Commit the transaction
        await _unitOfWork.SaveChangesAsync();

        // Return the ID of the created review
        return review.Id;
    }
}