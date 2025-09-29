using InzRate.Core.Domain.ValueObjects;
using InzRate.Core.Domain.Events;

namespace InzRate.Core.Domain.Aggregates;

public class Review
{
    private readonly List<object> _domainEvents = new();
    
    public Guid Id { get; private set; }
    public Guid MovieId { get; private set; }
    public Guid UserId { get; private set; }
    public Rating Rating { get; private set; } = default!;
    public string ReviewText { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }

    // Private constructor to prevent direct instantiation
    private Review()
    {
    }

    public static Review Create(Guid movieId, Guid userId, Rating rating, string text)
    {
        // Validate parameters
        if (movieId == Guid.Empty)
            throw new ArgumentException("MovieId cannot be empty.", nameof(movieId));
        
        if (userId == Guid.Empty)
            throw new ArgumentException("UserId cannot be empty.", nameof(userId));
        
        if (rating == null)
            throw new ArgumentNullException(nameof(rating));
        
        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentException("Review text cannot be null or empty.", nameof(text));

        // Create and return a new Review instance
        var review = new Review
        {
            Id = Guid.NewGuid(),  // Generate a new ID for the review
            MovieId = movieId,
            UserId = userId,
            Rating = rating,
            ReviewText = text,
            CreatedAt = DateTime.UtcNow
        };

        // Add the domain event to be raised
        var reviewCreatedEvent = new ReviewCreatedEvent(
            review.Id,
            review.MovieId,
            review.UserId,
            review.Rating,
            review.ReviewText,
            review.CreatedAt
        );
        
        review.AddDomainEvent(reviewCreatedEvent);
        
        return review;
    }

    // Method to add domain events
    private void AddDomainEvent(object eventItem)
    {
        _domainEvents.Add(eventItem);
    }

    // Method to get domain events
    public IReadOnlyList<object> GetDomainEvents() => _domainEvents.AsReadOnly();

    // Method to clear domain events after they've been processed
    public void ClearDomainEvents() => _domainEvents.Clear();
}