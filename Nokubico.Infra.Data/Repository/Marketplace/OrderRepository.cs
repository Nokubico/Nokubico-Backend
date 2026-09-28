using System;
using Microsoft.EntityFrameworkCore;
using Nokubico.Domain.Entities;
using Nokubico.Domain.Interface.Marketplace;
using Nokubico.Domain.Pagination;
using Nokubico.Infra.Data.Context;

namespace Nokubico.Infra.Data.Repository.Marketplace
{
    public class OrderRepository : BaseRepository, IOrderRepository
    {
        public OrderRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Order?> FindById(Guid id, CancellationToken cancellationToken = default)
        {
            return await Context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
        }

        public async Task<Order?> FindByIdForUser(Guid id, Guid userId, CancellationToken cancellationToken = default)
        {
            return await Context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == id && o.UserId == userId, cancellationToken);
        }

        public async Task<PagedList<Order>> FindByBuyer(Guid userId, PaginationParams pagination, CancellationToken cancellationToken = default)
        {
            var query = Context.Orders.Where(o => o.UserId == userId);
            var total = await query.CountAsync(cancellationToken);
            var items = await query
                .Include(o => o.Items)
                .OrderByDescending(o => o.CreatedAt)
                .Skip(pagination.Offset)
                .Take(pagination.PageSize)
                .ToListAsync(cancellationToken);

            return ToPagedList(items, total, pagination);
        }

        public async Task<PagedList<Order>> FindBySeller(Guid creatorId, PaginationParams pagination, CancellationToken cancellationToken = default)
        {
            var query = Context.Orders.Where(o => o.Items.Any(i => i.Product != null && i.Product.CreatorId == creatorId));
            var total = await query.CountAsync(cancellationToken);
            var items = await query
                .Include(o => o.Items)
                .OrderByDescending(o => o.CreatedAt)
                .Skip(pagination.Offset)
                .Take(pagination.PageSize)
                .ToListAsync(cancellationToken);

            return ToPagedList(items, total, pagination);
        }

        public async Task<Order> Save(Order order, CancellationToken cancellationToken = default)
        {
            if (!Context.Orders.Contains(order)) Context.Orders.Add(order);
            await Context.SaveChangesAsync(cancellationToken);
            return order;
        }

        public async Task Delete(Order order, CancellationToken cancellationToken = default)
        {
            Context.Orders.Remove(order);
            await Context.SaveChangesAsync(cancellationToken);
        }
    }
}