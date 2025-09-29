namespace InzRate.Core.Application.DTOs;

public record ReviewDto(
    Guid Id,
    Guid MovieId,
    Guid UserId,
    int RatingValue,
    string ReviewText,
    string Username, // Associated with the user who wrote the review
    DateTime CreatedAt
);