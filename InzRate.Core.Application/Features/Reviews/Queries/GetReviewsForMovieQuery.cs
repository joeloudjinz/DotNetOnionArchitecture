using MediatR;
using InzRate.Core.Application.DTOs;

namespace InzRate.Core.Application.Features.Reviews.Queries;

public record GetReviewsForMovieQuery(Guid MovieId) : IRequest<IEnumerable<ReviewDto>>;