using System;

namespace Nokubico.Domain.Entities
{
    public class CompanyMember
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public Guid UserId { get; set; }
        public string? Role { get; set; }
        public DateTime JoinedAt { get; set; }
    }
}
