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

        public Company? FindById(Guid id)
        {
            return Context.Companies
                .Include(c => c.Members)
                .Include(c => c.Follows)
                .FirstOrDefault(c => c.Id == id);
        }

        public PagedList<Company> FindAll(PaginationParams pagination)
        {
            var total = Context.Companies.Count();
            var items = Context.Companies
                .OrderBy(c => c.CreatedAt)
                .Skip(pagination.Offset)
                .Take(pagination.PageSize)
                .ToList();

            return ToPagedList(items, total, pagination);
        }

        public Company Save(Company company)
        {
            if (!Context.Companies.Contains(company)) Context.Companies.Add(company);
            Context.SaveChanges();
            return company;
        }

        public void Delete(Company company)
        {
            Context.Companies.Remove(company);
            Context.SaveChanges();
        }

        public CompanyMember? FindMember(Guid companyId, Guid userId)
        {
            return Context.CompanyMembers.FirstOrDefault(m => m.CompanyId == companyId && m.UserId == userId);
        }

        public CompanyMember SaveMember(CompanyMember member)
        {
            if (!Context.CompanyMembers.Contains(member)) Context.CompanyMembers.Add(member);
            Context.SaveChanges();
            return member;
        }

        public void DeleteMember(CompanyMember member)
        {
            Context.CompanyMembers.Remove(member);
            Context.SaveChanges();
        }

        public CompanyFollow SaveFollow(CompanyFollow follow)
        {
            if (!Context.CompanyFollows.Contains(follow)) Context.CompanyFollows.Add(follow);
            Context.SaveChanges();
            return follow;
        }

        public void DeleteFollow(CompanyFollow follow)
        {
            Context.CompanyFollows.Remove(follow);
            Context.SaveChanges();
        }

        public bool IsFollowing(Guid companyId, Guid userId)
        {
            return Context.CompanyFollows.Any(f => f.CompanyId == companyId && f.UserId == userId);
        }
    }
}