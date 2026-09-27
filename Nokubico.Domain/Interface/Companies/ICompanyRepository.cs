using System;
using Nokubico.Domain.Entities;
using Nokubico.Domain.Pagination;

namespace Nokubico.Domain.Interface.Companies
{
    public interface ICompanyRepository
    {
        Company? FindById(Guid id);

        PagedList<Company> FindAll(PaginationParams pagination);

        Company Save(Company company);

        void Delete(Company company);

        CompanyMember? FindMember(Guid companyId, Guid userId);

        CompanyMember SaveMember(CompanyMember member);

        void DeleteMember(CompanyMember member);

        CompanyFollow SaveFollow(CompanyFollow follow);

        void DeleteFollow(CompanyFollow follow);

        bool IsFollowing(Guid companyId, Guid userId);
    }
}