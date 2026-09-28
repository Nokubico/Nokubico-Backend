using System;
using Nokubico.Domain.Entities;
using Nokubico.Domain.Pagination;

namespace Nokubico.Domain.Interface.Auth
{
    public interface IUserRepository
    {
        Task<User?> FindByEmail(string email, CancellationToken cancellationToken = default);

        Task<User?> FindById(Guid id, CancellationToken cancellationToken = default);

        Task<User> Save(User user, CancellationToken cancellationToken = default);

        Task Delete(User user, CancellationToken cancellationToken = default);

        Task<PagedList<User>> FindAll(PaginationParams pagination, CancellationToken cancellationToken = default);
    }
}