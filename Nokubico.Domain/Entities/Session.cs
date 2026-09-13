using System;

namespace Nokubico.Domain.Entities
{
    public class Session : BaseEntity
    {
        public DateTime ExpiresAt { get; private set; }
        public string Token { get; private set; } = null!;
        public Guid UserId { get; private set; }

        public User? User { get; private set; }

        public void SetExpiresAt(DateTime expires)
        {
            ExpiresAt = expires;
            Touch();
        }

        public void SetToken(string token)
        {
            Token = token;
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
