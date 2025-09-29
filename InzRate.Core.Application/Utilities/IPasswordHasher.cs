namespace InzRate.Core.Application.Utilities;

public interface IPasswordHasher
{
    (string hash, string salt) HashPasswordWithSalt(string password);
    bool VerifyPassword(string password, string hashedPassword, string salt);
}