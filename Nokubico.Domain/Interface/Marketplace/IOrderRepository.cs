using System;
using Nokubico.Domain.Entities;
using Nokubico.Domain.Pagination;

namespace Nokubico.Domain.Interface.Marketplace
{
    public interface IOrderRepository
    {
        Order? FindById(Guid id);

        Order? FindByIdForUser(Guid id, Guid userId);

        PagedList<Order> FindByBuyer(Guid userId, PaginationParams pagination);

        PagedList<Order> FindBySeller(Guid creatorId, PaginationParams pagination);

        Order Save(Order order);

        void Delete(Order order);
    }
}