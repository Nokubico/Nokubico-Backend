using System;
using Nokubico.Domain.Entities;
using Nokubico.Domain.Enums;

namespace Nokubico.Domain.Account
{
    public interface IAuthenticate
    {
        bool UserExists(string email);

        User? Register(string name, string email, string password);

        User? GetUserByEmail(string email);

        bool Authenticate(string email, string password);

        string GenerateToken(Guid id, string email, UserRole role);
    }
}