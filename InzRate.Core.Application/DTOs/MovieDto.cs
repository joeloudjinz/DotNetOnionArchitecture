namespace InzRate.Core.Application.DTOs;

public record MovieDto(
    Guid Id,
    string Title,
    int ReleaseYear,
    double? AverageRating,
    int ReviewCount
);