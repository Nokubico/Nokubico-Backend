using System;
using Microsoft.EntityFrameworkCore;
using Nokubico.Domain.Entities;
using Nokubico.Domain.Interface.Companies;
using Nokubico.Domain.Pagination;
using Nokubico.Infra.Data.Context;

namespace Nokubico.Infra.Data.Repository.Companies
{
    public class CompanyRepository : BaseRepository, ICompanyRepository
    {
        public CompanyRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Company?> FindById(Guid id, CancellationToken cancellationToken = default)
        {
            return await Context.Companies
                .Include(c => c.Members)
                .Include(c => c.Follows)
                .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task<PagedList<Company>> FindAll(PaginationParams pagination, CancellationToken cancellationToken = default)
        {
            var total = await Context.Companies.CountAsync(cancellationToken);
            var items = await Context.Companies
                .OrderBy(c => c.CreatedAt)
                .Skip(pagination.Offset)
                .Take(pagination.PageSize)
                .ToListAsync(cancellationToken);

            return ToPagedList(items, total, pagination);
        }

        public async Task<Company> Save(Company company, CancellationToken cancellationToken = default)
        {
            if (!Context.Companies.Contains(company)) Context.Companies.Add(company);
            await Context.SaveChangesAsync(cancellationToken);
            return company;
        }

        public async Task Delete(Company company, CancellationToken cancellationToken = default)
        {
            Context.Companies.Remove(company);
            await Context.SaveChangesAsync(cancellationToken);
        }

        public async Task<CompanyMember?> FindMember(Guid companyId, Guid userId, CancellationToken cancellationToken = default)
        {
            return await Context.CompanyMembers.FirstOrDefaultAsync(m => m.CompanyId == companyId && m.UserId == userId, cancellationToken);
        }

        public async Task<CompanyMember> SaveMember(CompanyMember member, CancellationToken cancellationToken = default)
        {
            if (!Context.CompanyMembers.Contains(member)) Context.CompanyMembers.Add(member);
            await Context.SaveChangesAsync(cancellationToken);
            return member;
        }

        public async Task DeleteMember(CompanyMember member, CancellationToken cancellationToken = default)
        {
            Context.CompanyMembers.Remove(member);
            await Context.SaveChangesAsync(cancellationToken);
        }

        public async Task<CompanyFollow> SaveFollow(CompanyFollow follow, CancellationToken cancellationToken = default)
        {
            if (!Context.CompanyFollows.Contains(follow)) Context.CompanyFollows.Add(follow);
            await Context.SaveChangesAsync(cancellationToken);
            return follow;
        }

        public async Task DeleteFollow(CompanyFollow follow, CancellationToken cancellationToken = default)
        {
            Context.CompanyFollows.Remove(follow);
            await Context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> IsFollowing(Guid companyId, Guid userId, CancellationToken cancellationToken = default)
        {
            return await Context.CompanyFollows.AnyAsync(f => f.CompanyId == companyId && f.UserId == userId, cancellationToken);
        }
    }
}