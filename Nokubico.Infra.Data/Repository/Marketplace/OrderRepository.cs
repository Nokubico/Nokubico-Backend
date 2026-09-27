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

        public Order? FindById(Guid id)
        {
            return Context.Orders
                .Include(o => o.Items)
                .FirstOrDefault(o => o.Id == id);
        }

        public Order? FindByIdForUser(Guid id, Guid userId)
        {
            return Context.Orders
                .Include(o => o.Items)
                .FirstOrDefault(o => o.Id == id && o.UserId == userId);
        }

        public PagedList<Order> FindByBuyer(Guid userId, PaginationParams pagination)
        {
            var query = Context.Orders.Where(o => o.UserId == userId);
            var total = query.Count();
            var items = query
                .Include(o => o.Items)
                .OrderByDescending(o => o.CreatedAt)
                .Skip(pagination.Offset)
                .Take(pagination.PageSize)
                .ToList();

            return ToPagedList(items, total, pagination);
        }

        public PagedList<Order> FindBySeller(Guid creatorId, PaginationParams pagination)
        {
            var query = Context.Orders.Where(o => o.Items.Any(i => i.Product.CreatorId == creatorId));
            var total = query.Count();
            var items = query
                .Include(o => o.Items)
                .OrderByDescending(o => o.CreatedAt)
                .Skip(pagination.Offset)
                .Take(pagination.PageSize)
                .ToList();

            return ToPagedList(items, total, pagination);
        }

        public Order Save(Order order)
        {
            if (!Context.Orders.Contains(order)) Context.Orders.Add(order);
            Context.SaveChanges();
            return order;
        }

        public void Delete(Order order)
        {
            Context.Orders.Remove(order);
            Context.SaveChanges();
        }
    }
}