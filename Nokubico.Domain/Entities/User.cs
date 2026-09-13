using System;
using System.Collections.Generic;
using Nokubico.Domain.Enums;

namespace Nokubico.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Email { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Image { get; set; }
        public string? Bio { get; set; }
        public string? Location { get; set; }
        public string? Profession { get; set; }
        public UserRole Role { get; set; }
        public bool EmailVerified { get; set; }

        public ICollection<Account>? Accounts { get; set; }
        public ICollection<Session>? Sessions { get; set; }
        public ICollection<Post>? Posts { get; set; }
    }
}
