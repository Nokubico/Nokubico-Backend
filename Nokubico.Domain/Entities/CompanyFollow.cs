using System;

namespace Nokubico.Domain.Entities
{
    public class CompanyFollow : IEntity
    {
        public Guid Id { get; private set; }
        public Guid CompanyId { get; private set; }
        public Guid UserId { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public Company? Company { get; private set; }
        public User? User { get; private set; }

        public CompanyFollow()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
        }

        public CompanyFollow(Company company, User user)
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            SetCompany(company);
            SetUser(user);
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

        public void SetCompanyAndUser(Guid companyId, Guid userId)
        {
            CompanyId = companyId;
            UserId = userId;
        }
    }
}
