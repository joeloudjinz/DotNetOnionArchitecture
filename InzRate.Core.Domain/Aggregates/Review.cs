using InzRate.Core.Domain.ValueObjects;

namespace InzRate.Core.Domain.Aggregates;

public class Review
{
    public int Id { get; set; }
    public int MovieId { get; set; }
    public int UserId { get; set; }
    public Rating Rating { get; set; } = default!;
    public string ReviewText { get; set; } = string.Empty;
}