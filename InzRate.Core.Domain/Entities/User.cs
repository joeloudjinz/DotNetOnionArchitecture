namespace InzRate.Core.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Username { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;

    // Private constructor to enforce factory method usage
    private User()
    {
    }

    public static User Create(string username, string email)
    {
        Validate(username, email);


        return new User
        {
            Id = Guid.NewGuid(),
            Username = username,
            Email = email
        };
    }

    public static User Create(Guid id, string username, string email)
    {
        Validate(username, email);

        return new User
        {
            Id = id,
            Username = username,
            Email = email
        };
    }

    public User Update(string username, string email)
    {
        Validate(username, email);

        return new User
        {
            Username = username,
            Email = email
        };
    }

    private static void Validate(string username, string email)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new ArgumentException("Username cannot be empty.", nameof(username));
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Email cannot be empty.", nameof(email));
        }
    }
}