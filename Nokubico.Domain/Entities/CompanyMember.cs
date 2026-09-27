using System;

namespace Nokubico.Domain.Entities
{
    public class CompanyMember : IEntity
    {
        public Guid Id { get; private set; }
        public Guid CompanyId { get; private set; }
        public Guid UserId { get; private set; }
        public string? Role { get; private set; }
        public DateTime JoinedAt { get; private set; }

        public Company? Company { get; private set; }
        public User? User { get; private set; }

        public CompanyMember()
        {
            Id = Guid.NewGuid();
            JoinedAt = DateTime.UtcNow;
        }

        public CompanyMember(Company company, User user, string? role)
        {
            Id = Guid.NewGuid();
            JoinedAt = DateTime.UtcNow;
            SetCompany(company);
            SetUser(user);
            Role = role;
        }

        public void SetCompany(Company company)
        {
            Company = company;
            CompanyId = company.Id;
        }

        public void SetUser(User user)
        {
            User = user;
            UserId = user.Id;
        }

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
