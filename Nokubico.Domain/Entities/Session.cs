using System;

namespace Nokubico.Domain.Entities
{
    public class Session : BaseEntity
    {
        public DateTime ExpiresAt { get; set; }
        public string Token { get; set; } = null!;
        public Guid UserId { get; set; }

        public User? User { get; set; }
    }
}
