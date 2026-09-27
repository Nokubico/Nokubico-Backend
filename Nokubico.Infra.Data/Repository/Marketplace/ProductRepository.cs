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

        public Product? FindById(Guid id)
        {
            return Context.Products
                .Include(p => p.Images)
                .Include(p => p.Creator)
                .FirstOrDefault(p => p.Id == id);
        }

        public Product? FindPublishedById(Guid id)
        {
            return Context.Products
                .Include(p => p.Images)
                .Include(p => p.Creator)
                .FirstOrDefault(p => p.Id == id && p.Status == ProductStatus.Published);
        }

        public PagedList<Product> FindPublished(string? category, PaginationParams pagination)
        {
            var query = Context.Products.Where(p => p.Status == ProductStatus.Published);

            if (category != null && category.Trim().Length > 0)
            {
                query = query.Where(p => p.Category == category);
            }

            var total = query.Count();
            var items = query
                .Include(p => p.Images)
                .OrderByDescending(p => p.CreatedAt)
                .Skip(pagination.Offset)
                .Take(pagination.PageSize)
                .ToList();

            return ToPagedList(items, total, pagination);
        }

        public PagedList<Product> FindByCreator(Guid creatorId, PaginationParams pagination)
        {
            var query = Context.Products.Where(p => p.CreatorId == creatorId);
            var total = query.Count();
            var items = query
                .Include(p => p.Images)
                .OrderByDescending(p => p.CreatedAt)
                .Skip(pagination.Offset)
                .Take(pagination.PageSize)
                .ToList();

            return ToPagedList(items, total, pagination);
        }

        public Product Save(Product product)
        {
            if (!Context.Products.Contains(product)) Context.Products.Add(product);
            Context.SaveChanges();
            return product;
        }

        public void Delete(Product product)
        {
            Context.Products.Remove(product);
            Context.SaveChanges();
        }

        public Review? FindReview(Guid productId, Guid userId)
        {
            return Context.Reviews.FirstOrDefault(r => r.ProductId == productId && r.UserId == userId);
        }

        public Review SaveReview(Review review)
        {
            if (!Context.Reviews.Contains(review)) Context.Reviews.Add(review);
            Context.SaveChanges();
            return review;
        }

        public void DeleteReview(Review review)
        {
            Context.Reviews.Remove(review);
            Context.SaveChanges();
        }

        public double GetAverageRating(Guid productId)
        {
            var ratings = Context.Reviews
                .Where(r => r.ProductId == productId)
                .Select(r => r.Rating)
                .ToList();

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

        public PagedList<Review> FindReviewsByProduct(Guid productId, PaginationParams pagination)
        {
            var query = Context.Reviews.Where(r => r.ProductId == productId);
            var total = query.Count();
            var items = query
                .Include(r => r.User)
                .OrderByDescending(r => r.CreatedAt)
                .Skip(pagination.Offset)
                .Take(pagination.PageSize)
                .ToList();

            return ToPagedList(items, total, pagination);
        }
    }
}