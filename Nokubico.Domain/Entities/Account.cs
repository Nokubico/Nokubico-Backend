using System;

namespace Nokubico.Domain.Entities
{
    public class Account : BaseEntity
    {
        public string AccountId { get; private set; } = null!;
        public string ProviderId { get; private set; } = null!;
        public Guid UserId { get; private set; }
        public string? AccessToken { get; private set; }
        public string? RefreshToken { get; private set; }
        public string? Password { get; private set; }

        public User? User { get; private set; }

        public void SetTokens(string? accessToken, string? refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            Touch();
        }

        public void SetPassword(string? password)
        {
            Password = password;
            Touch();
        }

        public void SetUser(User user)
        {
            User = user;
            UserId = user.Id;
            Touch();
        }
    }
}
