using System;

namespace InzRate.Core.Domain.ValueObjects;

public class Rating : IEquatable<Rating>
{
    public int Value { get; }

    public Rating(int value)
    {
        if (value < 1 || value > 5)
        {
            throw new ArgumentException("Rating must be between 1 and 5.", nameof(value));
        }
        
        Value = value;
    }

    public bool Equals(Rating? other)
    {
        if (other is null)
            return false;
        
        if (ReferenceEquals(this, other))
            return true;
        
        return Value.Equals(other.Value);
    }

    public override bool Equals(object? obj)
    {
        return obj is Rating rating && Equals(rating);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public static bool operator ==(Rating? left, Rating? right)
    {
        if (left is null && right is null)
            return true;
        
        if (left is null || right is null)
            return false;
        
        return left.Equals(right);
    }

    public static bool operator !=(Rating? left, Rating? right)
    {
        return !(left == right);
    }
}