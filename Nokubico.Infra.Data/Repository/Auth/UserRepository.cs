using System;
using Microsoft.EntityFrameworkCore;
using Nokubico.Domain.Entities;
using Nokubico.Domain.Interface.Auth;
using Nokubico.Domain.Pagination;
using Nokubico.Infra.Data.Context;

namespace Nokubico.Infra.Data.Repository.Auth
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context)
        {
        }

        public User? FindByEmail(string email)
        {
            return Context.Users.FirstOrDefault(u => u.Email == email);
        }

        public User? FindById(Guid id)
        {
            return Context.Users.FirstOrDefault(u => u.Id == id);
        }

        public User Save(User user)
        {
            if (!Context.Users.Contains(user)) Context.Users.Add(user);
            Context.SaveChanges();
            return user;
        }

        public void Delete(User user)
        {
            Context.Users.Remove(user);
            Context.SaveChanges();
        }

        public PagedList<User> FindAll(PaginationParams pagination)
        {
            var total = Context.Users.Count();
            var items = Context.Users
                .OrderBy(u => u.CreatedAt)
                .Skip(pagination.Offset)
                .Take(pagination.PageSize)
                .ToList();

            return ToPagedList(items, total, pagination);
        }
    }
}