using System;
using System.Collections.Generic;

namespace Nokubico.Domain.Entities
{
    public class Company
    {
        public Guid Id { get; private set; }
        public string? Name { get; private set; }
        public string? Description { get; private set; }
        public string? Website { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        public ICollection<CompanyMember> Members { get; private set; } = new List<CompanyMember>();
        public ICollection<CompanyFollow> Follows { get; private set; } = new List<CompanyFollow>();

        public void SetName(string? name)
        {
            Name = name;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetDescription(string? description)
        {
            Description = description;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetWebsite(string? website)
        {
            Website = website;
            UpdatedAt = DateTime.UtcNow;
        }

        public void AddMember(CompanyMember member)
        {
            if (member != null) Members.Add(member);
        }

        public void AddFollow(CompanyFollow follow)
        {
            if (follow != null) Follows.Add(follow);
        }
    }
}
