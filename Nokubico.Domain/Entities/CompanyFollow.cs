using System;

namespace Nokubico.Domain.Entities
{
    public class CompanyFollow
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
