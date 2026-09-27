using System;
using Nokubico.Domain.Entities;
using Nokubico.Domain.Pagination;

namespace Nokubico.Domain.Interface.Marketplace
{
    public interface IProductRepository
    {
        Product? FindById(Guid id);

        Product? FindPublishedById(Guid id);

        PagedList<Product> FindPublished(string? category, PaginationParams pagination);

        PagedList<Product> FindByCreator(Guid creatorId, PaginationParams pagination);

        Product Save(Product product);

        void Delete(Product product);

        Review? FindReview(Guid productId, Guid userId);

        Review SaveReview(Review review);

        void DeleteReview(Review review);

        double GetAverageRating(Guid productId);

        PagedList<Review> FindReviewsByProduct(Guid productId, PaginationParams pagination);
    }
}