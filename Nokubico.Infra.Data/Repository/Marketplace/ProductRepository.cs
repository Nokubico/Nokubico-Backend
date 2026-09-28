using System;
using Microsoft.EntityFrameworkCore;
using Nokubico.Domain.Entities;
using Nokubico.Domain.Enums;
using Nokubico.Domain.Interface.Marketplace;
using Nokubico.Domain.Pagination;
using Nokubico.Infra.Data.Context;

namespace Nokubico.Infra.Data.Repository.Marketplace
{
    public class ProductRepository : BaseRepository, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Product?> FindById(Guid id, CancellationToken cancellationToken = default)
        {
            return await Context.Products
                .Include(p => p.Images)
                .Include(p => p.Creator)
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<Product?> FindPublishedById(Guid id, CancellationToken cancellationToken = default)
        {
            return await Context.Products
                .Include(p => p.Images)
                .Include(p => p.Creator)
                .FirstOrDefaultAsync(p => p.Id == id && p.Status == ProductStatus.Published, cancellationToken);
        }

        public async Task<PagedList<Product>> FindPublished(string? category, PaginationParams pagination, CancellationToken cancellationToken = default)
        {
            var query = Context.Products.Where(p => p.Status == ProductStatus.Published);

            if (category != null && category.Trim().Length > 0)
            {
                query = query.Where(p => p.Category == category);
            }

            var total = await query.CountAsync(cancellationToken);
            var items = await query
                .Include(p => p.Images)
                .OrderByDescending(p => p.CreatedAt)
                .Skip(pagination.Offset)
                .Take(pagination.PageSize)
                .ToListAsync(cancellationToken);

            return ToPagedList(items, total, pagination);
        }

        public async Task<PagedList<Product>> FindByCreator(Guid creatorId, PaginationParams pagination, CancellationToken cancellationToken = default)
        {
            var query = Context.Products.Where(p => p.CreatorId == creatorId);
            var total = await query.CountAsync(cancellationToken);
            var items = await query
                .Include(p => p.Images)
                .OrderByDescending(p => p.CreatedAt)
                .Skip(pagination.Offset)
                .Take(pagination.PageSize)
                .ToListAsync(cancellationToken);

            return ToPagedList(items, total, pagination);
        }

        public async Task<Product> Save(Product product, CancellationToken cancellationToken = default)
        {
            if (!Context.Products.Contains(product)) Context.Products.Add(product);
            await Context.SaveChangesAsync(cancellationToken);
            return product;
        }

        public async Task Delete(Product product, CancellationToken cancellationToken = default)
        {
            Context.Products.Remove(product);
            await Context.SaveChangesAsync(cancellationToken);
        }

        public async Task<Review?> FindReview(Guid productId, Guid userId, CancellationToken cancellationToken = default)
        {
            return await Context.Reviews.FirstOrDefaultAsync(r => r.ProductId == productId && r.UserId == userId, cancellationToken);
        }

        public async Task<Review> SaveReview(Review review, CancellationToken cancellationToken = default)
        {
            if (!Context.Reviews.Contains(review)) Context.Reviews.Add(review);
            await Context.SaveChangesAsync(cancellationToken);
            return review;
        }

        public async Task DeleteReview(Review review, CancellationToken cancellationToken = default)
        {
            Context.Reviews.Remove(review);
            await Context.SaveChangesAsync(cancellationToken);
        }

        public async Task<double> GetAverageRating(Guid productId, CancellationToken cancellationToken = default)
        {
            var ratings = await Context.Reviews
                .Where(r => r.ProductId == productId)
                .Select(r => r.Rating)
                .ToListAsync(cancellationToken);

            if (ratings.Count == 0)
            {
                return 0.0;
            }

            var sum = 0;
            foreach (var rating in ratings)
            {
                sum += rating;
            }

            return (double)sum / ratings.Count;
        }

        public async Task<PagedList<Review>> FindReviewsByProduct(Guid productId, PaginationParams pagination, CancellationToken cancellationToken = default)
        {
            var query = Context.Reviews.Where(r => r.ProductId == productId);
            var total = await query.CountAsync(cancellationToken);
            var items = await query
                .Include(r => r.User)
                .OrderByDescending(r => r.CreatedAt)
                .Skip(pagination.Offset)
                .Take(pagination.PageSize)
                .ToListAsync(cancellationToken);

            return ToPagedList(items, total, pagination);
        }
    }
}