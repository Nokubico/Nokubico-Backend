using System;
using Nokubico.Domain.Entities;
using Nokubico.Domain.Pagination;

namespace Nokubico.Domain.Interface.Companies
{
    public interface ICompanyRepository
    {
        Task<Company?> FindById(Guid id, CancellationToken cancellationToken = default);

        Task<PagedList<Company>> FindAll(PaginationParams pagination, CancellationToken cancellationToken = default);

        Task<Company> Save(Company company, CancellationToken cancellationToken = default);

        Task Delete(Company company, CancellationToken cancellationToken = default);

        Task<CompanyMember?> FindMember(Guid companyId, Guid userId, CancellationToken cancellationToken = default);

        Task<CompanyMember> SaveMember(CompanyMember member, CancellationToken cancellationToken = default);

        Task DeleteMember(CompanyMember member, CancellationToken cancellationToken = default);

        Task<CompanyFollow> SaveFollow(CompanyFollow follow, CancellationToken cancellationToken = default);

        Task DeleteFollow(CompanyFollow follow, CancellationToken cancellationToken = default);

        Task<bool> IsFollowing(Guid companyId, Guid userId, CancellationToken cancellationToken = default);
    }
}