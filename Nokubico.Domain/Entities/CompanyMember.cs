using System;

namespace Nokubico.Domain.Entities
{
    public class CompanyMember
    {
        public Guid Id { get; private set; }
        public Guid CompanyId { get; private set; }
        public Guid UserId { get; private set; }
        public string? Role { get; private set; }
        public DateTime JoinedAt { get; private set; }

        public void SetRole(string? role)
        {
            Role = role;
        }

        public void SetJoinedAt(DateTime at)
        {
            JoinedAt = at;
        }
    }
}
