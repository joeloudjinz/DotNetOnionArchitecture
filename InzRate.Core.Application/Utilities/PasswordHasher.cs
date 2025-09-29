using System.Security.Cryptography;
using System.Text;

namespace InzRate.Core.Application.Utilities;

public class PasswordHasher : IPasswordHasher
{
    // Generate a random salt for each password
    public (string hash, string salt) HashPasswordWithSalt(string password)
    {
        if (string.IsNullOrEmpty(password))
            throw new ArgumentException("Password cannot be null or empty.", nameof(password));

        // Generate a random salt
        var saltBytes = new byte[32]; // 256 bits
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(saltBytes);
        }
        
        var salt = Convert.ToBase64String(saltBytes);

        // Hash the password with the salt
        var hashedBytes = ComputeHash(password, salt);
        var hash = Convert.ToBase64String(hashedBytes);

        return (hash, salt);
    }

    public bool VerifyPassword(string password, string hashedPassword, string salt)
    {
        if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(hashedPassword) || string.IsNullOrEmpty(salt))
            return false;

        // Hash the provided password with the same salt
        var hashedInput = Convert.ToBase64String(ComputeHash(password, salt));

        // Compare the hashed input with the stored hash
        return hashedInput == hashedPassword;
    }

    private byte[] ComputeHash(string password, string salt)
    {
        // Combine password and salt
        var combinedString = password + salt;

        // Use SHA256 to hash the combined string
        using (var sha256 = SHA256.Create())
        {
            return sha256.ComputeHash(Encoding.UTF8.GetBytes(combinedString));
        }
    }
}