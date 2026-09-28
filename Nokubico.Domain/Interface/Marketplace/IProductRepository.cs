using System;
using Nokubico.Domain.Entities;
using Nokubico.Domain.Pagination;

namespace Nokubico.Domain.Interface.Marketplace
{
    public interface IProductRepository
    {
        Task<Product?> FindById(Guid id, CancellationToken cancellationToken = default);

        Task<Product?> FindPublishedById(Guid id, CancellationToken cancellationToken = default);

        Task<PagedList<Product>> FindPublished(string? category, PaginationParams pagination, CancellationToken cancellationToken = default);

        Task<PagedList<Product>> FindByCreator(Guid creatorId, PaginationParams pagination, CancellationToken cancellationToken = default);

        Task<Product> Save(Product product, CancellationToken cancellationToken = default);

        Task Delete(Product product, CancellationToken cancellationToken = default);

        Task<Review?> FindReview(Guid productId, Guid userId, CancellationToken cancellationToken = default);

        Task<Review> SaveReview(Review review, CancellationToken cancellationToken = default);

        Task DeleteReview(Review review, CancellationToken cancellationToken = default);

        Task<double> GetAverageRating(Guid productId, CancellationToken cancellationToken = default);

        Task<PagedList<Review>> FindReviewsByProduct(Guid productId, PaginationParams pagination, CancellationToken cancellationToken = default);
    }
}