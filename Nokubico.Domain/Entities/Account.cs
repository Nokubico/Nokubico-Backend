using System;

namespace Nokubico.Domain.Entities
{
    public class Account : BaseEntity
    {
        public string AccountId { get; set; } = null!;
        public string ProviderId { get; set; } = null!;
        public Guid UserId { get; set; }
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public string? Password { get; set; }

        public User? User { get; set; }
    }
}
