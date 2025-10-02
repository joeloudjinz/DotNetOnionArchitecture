using InzRate.Core.Application.Contracts.Persistence;
using InzRate.Core.Application.DTOs;
using MediatR;

namespace InzRate.Core.Application.Features.Reviews.Queries;

public class GetReviewsForMovieQueryHandler(IUserRepository userRepository, IReviewRepository reviewRepository) : IRequestHandler<GetReviewsForMovieQuery, IEnumerable<ReviewDto>>
{
    public async Task<IEnumerable<ReviewDto>> Handle(GetReviewsForMovieQuery request, CancellationToken cancellationToken)
    {
        // Get all reviews for the specified movie
        var reviews = await reviewRepository.GetByMovieIdAsync(request.MovieId);

        // Create a list to hold the DTOs
        var reviewDtos = new List<ReviewDto>();

        // Map each review to a DTO
        foreach (var review in reviews)
        {
            // Get the associated user to include the username in the DTO
            var user = await userRepository.GetByIdAsync(review.UserId);

            var reviewDto = new ReviewDto(
                Id: review.Id,
                MovieId: review.MovieId,
                UserId: review.UserId,
                RatingValue: review.Rating.Value, // Extract the value from the Rating value object
                ReviewText: review.ReviewText,
                Username: user?.Username ?? "Unknown", // Use "Unknown" if user is not found
                CreatedAt: review.CreatedAt
            );

            reviewDtos.Add(reviewDto);
        }

        return reviewDtos;
    }
}