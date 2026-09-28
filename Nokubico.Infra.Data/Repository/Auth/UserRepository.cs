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

        public async Task<User?> FindByEmail(string email, CancellationToken cancellationToken = default)
        {
            return await Context.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        }

        public async Task<User?> FindById(Guid id, CancellationToken cancellationToken = default)
        {
            return await Context.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        }

        public async Task<User> Save(User user, CancellationToken cancellationToken = default)
        {
            if (!Context.Users.Contains(user)) Context.Users.Add(user);
            await Context.SaveChangesAsync(cancellationToken);
            return user;
        }

        public async Task Delete(User user, CancellationToken cancellationToken = default)
        {
            Context.Users.Remove(user);
            await Context.SaveChangesAsync(cancellationToken);
        }

        public async Task<PagedList<User>> FindAll(PaginationParams pagination, CancellationToken cancellationToken = default)
        {
            var total = await Context.Users.CountAsync(cancellationToken);
            var items = await Context.Users
                .OrderBy(u => u.CreatedAt)
                .Skip(pagination.Offset)
                .Take(pagination.PageSize)
                .ToListAsync(cancellationToken);

            return ToPagedList(items, total, pagination);
        }
    }
}