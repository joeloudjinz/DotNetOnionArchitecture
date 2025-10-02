using InzRate.Core.Application.Contracts.Persistence;
using InzRate.Core.Domain.Aggregates;
using InzRate.Core.Domain.ValueObjects;
using MediatR;

namespace InzRate.Core.Application.Features.Reviews.Commands.CreateReview;

public class CreateReviewCommandHandler(IUnitOfWork unitOfWork, IMovieRepository movieRepository, IReviewRepository reviewRepository) : IRequestHandler<CreateReviewCommand, Guid>
{
    public async Task<Guid> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
    {
        // Validate that the movie exists
        var movie = await movieRepository.GetByIdAsync(request.MovieId);
        if (movie == null)
        {
            throw new ArgumentException($"Movie with ID {request.MovieId} does not exist.", nameof(request.MovieId));
        }

        // Validate the user has not already rated this movie
        var hasUserRatedMovie = await reviewRepository.HasUserRatedMovieAsync(request.UserId, request.MovieId);
        if (hasUserRatedMovie)
        {
            throw new InvalidOperationException($"User {request.UserId} has already rated movie {request.MovieId}.");
        }

        // Create the Rating value object
        var rating = new Rating(request.Stars);

        // Create the Review aggregate using its factory method
        var review = Review.Create(request.MovieId, request.UserId, rating, request.Text);

        // Add the new review to the repository
        await reviewRepository.AddAsync(review);

        // Commit the transaction
        await unitOfWork.SaveChangesAsync();

        // Return the ID of the created review
        return review.Id;
    }
}