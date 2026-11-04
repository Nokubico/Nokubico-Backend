using Nokubico.Application.DTOs;
using Nokubico.Domain.Pagination;

namespace Nokubico.Application.Interfaces;

public interface IProductService
{
    Task<PagedList<ProductDTO>> GetPublished(
        string? category,
        PaginationParams pagination,
        CancellationToken cancellationToken = default);

    Task<ProductDTO> GetById(
        Guid id,
        Guid? currentUserId = null,
        CancellationToken cancellationToken = default);

    Task<ProductDTO> Create(
        Guid creatorId,
        CreateProductDTO dto,
        CancellationToken cancellationToken = default);

    Task<ProductDTO> Update(
        Guid productId,
        Guid userId,
        UpdateProductDTO dto,
        CancellationToken cancellationToken = default);

    Task Publish(Guid productId, Guid userId, CancellationToken cancellationToken = default);

    Task Disable(Guid productId, Guid userId, CancellationToken cancellationToken = default);

    Task Delete(Guid productId, Guid userId, CancellationToken cancellationToken = default);

    Task<PagedList<ProductDTO>> GetMyProducts(
        Guid creatorId,
        PaginationParams pagination,
        CancellationToken cancellationToken = default);

    Task<ReviewDTO> AddReview(
        Guid productId,
        Guid userId,
        CreateReviewDTO dto,
        CancellationToken cancellationToken = default);

    Task<double> GetAverageRating(Guid productId, CancellationToken cancellationToken = default);
}
