using System;
using Nokubico.Domain.Entities;
using Nokubico.Domain.Pagination;

namespace Nokubico.Domain.Interface.Marketplace
{
    public interface IOrderRepository
    {
        Task<Order?> FindById(Guid id, CancellationToken cancellationToken = default);

        Task<Order?> FindByIdForUser(Guid id, Guid userId, CancellationToken cancellationToken = default);

        Task<PagedList<Order>> FindByBuyer(Guid userId, PaginationParams pagination, CancellationToken cancellationToken = default);

        Task<PagedList<Order>> FindBySeller(Guid creatorId, PaginationParams pagination, CancellationToken cancellationToken = default);

        Task<Order> Save(Order order, CancellationToken cancellationToken = default);

        Task Delete(Order order, CancellationToken cancellationToken = default);
    }
}