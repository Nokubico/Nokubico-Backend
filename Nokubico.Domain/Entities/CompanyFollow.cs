using System;

namespace Nokubico.Domain.Entities
{
    public class CompanyFollow
    {
        public Guid Id { get; private set; }
        public Guid CompanyId { get; private set; }
        public Guid UserId { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public void SetCompanyAndUser(Guid companyId, Guid userId)
        {
            CompanyId = companyId;
            UserId = userId;
        }
    }
}
