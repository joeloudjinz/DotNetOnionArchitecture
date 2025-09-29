using AutoMapper;
using InzRate.Core.Domain.Aggregates;
using InzRate.Core.Domain.Entities;
using InzRate.Core.Application.DTOs;

namespace InzRate.Core.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Map Review aggregate to ReviewDto
        CreateMap<Review, ReviewDto>()
            .ForMember(dest => dest.RatingValue, opt => opt.MapFrom(src => src.Rating.Value))
            .ForMember(dest => dest.Username, opt => opt.Ignore()) // Username will be populated separately in the handler
            .ReverseMap()
            .ForMember(dest => dest.Rating, opt => opt.Ignore()); // Rating is created via factory method

        // Map Movie entity to MovieDto
        CreateMap<Movie, MovieDto>()
            .ReverseMap();

        // Map User entity to UserDto
        CreateMap<User, UserDto>()
            .ReverseMap();
    }
}