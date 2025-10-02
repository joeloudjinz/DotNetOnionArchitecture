using MediatR;
using InzRate.Core.Application.Contracts.Persistence;
using InzRate.Core.Application.DTOs;

namespace InzRate.Core.Application.Features.Movies.Queries.GetAllMovies;

public class GetAllMoviesQueryHandler(IUnitOfWork unitOfWork, IMovieRepository movieRepository, IReviewRepository reviewRepository) : IRequestHandler<GetAllMoviesQuery, IEnumerable<MovieDto>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<IEnumerable<MovieDto>> Handle(GetAllMoviesQuery request, CancellationToken cancellationToken)
    {
        // Get all movies
        var movies = await movieRepository.GetAllAsync();

        // Create a list to hold the DTOs
        var movieDtos = new List<MovieDto>();

        // Map each movie to a DTO with average rating and review count
        foreach (var movie in movies)
        {
            // Get average rating for this movie
            var averageRating = await movieRepository.GetAverageRatingAsync(movie.Id);
            
            // Get count of reviews for this movie
            var reviews = await reviewRepository.GetByMovieIdAsync(movie.Id);
            var reviewCount = reviews.Count();

            var movieDto = new MovieDto(
                Id: movie.Id,
                Title: movie.Title,
                ReleaseYear: movie.ReleaseYear,
                AverageRating: averageRating,
                ReviewCount: reviewCount
            );

            movieDtos.Add(movieDto);
        }

        return movieDtos;
    }
}