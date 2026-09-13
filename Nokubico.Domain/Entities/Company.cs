using System;
using System.Collections.Generic;

namespace Nokubico.Domain.Entities
{
    public class Company
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Website { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public ICollection<CompanyMember>? Members { get; set; }
        public ICollection<CompanyFollow>? Follows { get; set; }
    }
}
