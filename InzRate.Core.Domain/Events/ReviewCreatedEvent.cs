using InzRate.Core.Domain.ValueObjects;

namespace InzRate.Core.Domain.Events;

public record ReviewCreatedEvent(
    Guid ReviewId,
    Guid MovieId,
    Guid UserId,
    Rating Rating,
    string ReviewText,
    DateTime CreatedAt = default
);