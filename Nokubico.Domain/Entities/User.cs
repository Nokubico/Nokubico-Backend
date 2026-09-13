using System;
using System.Collections.Generic;
using Nokubico.Domain.Enums;

namespace Nokubico.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Email { get; private set; } = null!;
        public string Name { get; private set; } = null!;
        public string? Image { get; private set; }
        public string? Bio { get; private set; }
        public string? Location { get; private set; }
        public string? Profession { get; private set; }
        public UserRole Role { get; private set; }
        public bool EmailVerified { get; private set; }

        public ICollection<Account> Accounts { get; private set; } = new List<Account>();
        public ICollection<Session> Sessions { get; private set; } = new List<Session>();
        public ICollection<Post> Posts { get; private set; } = new List<Post>();

        public void SetEmail(string email)
        {
            Email = email;
            Touch();
        }

        public void SetName(string name)
        {
            Name = name;
            Touch();
        }

        public void SetImage(string? image)
        {
            Image = image;
            Touch();
        }

        public void SetBio(string? bio)
        {
            Bio = bio;
            Touch();
        }

        public void SetLocation(string? location)
        {
            Location = location;
            Touch();
        }

        public void SetProfession(string? profession)
        {
            Profession = profession;
            Touch();
        }

        public void SetRole(UserRole role)
        {
            Role = role;
            Touch();
        }

        public void VerifyEmail()
        {
            EmailVerified = true;
            Touch();
        }

        public void AddAccount(Account account)
        {
            if (account != null) Accounts.Add(account);
        }

        public void AddSession(Session session)
        {
            if (session != null) Sessions.Add(session);
        }

        public void AddPost(Post post)
        {
            if (post != null) Posts.Add(post);
        }
    }
}
