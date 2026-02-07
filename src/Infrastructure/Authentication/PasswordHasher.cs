using System.Security.Cryptography;
using Application.Abstractions.Authentication;

namespace Infrastructure.Authentication;

internal sealed class PasswordHasher : IPasswordHasher
{
    public string Hash(string password) => 
        BCrypt.Net.BCrypt.HashPassword(password);
    
    public bool Verify(string password, string passwordHash)
    {
        return BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }
}
