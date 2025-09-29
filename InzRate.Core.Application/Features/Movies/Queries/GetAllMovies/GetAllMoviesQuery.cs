using MediatR;
using InzRate.Core.Application.DTOs;

namespace InzRate.Core.Application.Features.Movies.Queries.GetAllMovies;

public record GetAllMoviesQuery : IRequest<IEnumerable<MovieDto>>;