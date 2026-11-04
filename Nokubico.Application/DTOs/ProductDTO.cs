using Nokubico.Domain.Enums;

namespace Nokubico.Application.DTOs;

public record ProductDTO(
    Guid Id,
    string? Title,
    string? Slug,
    string? Description,
    string? Category,
    long Price,
    string Currency,
    string? ThumbnailUrl,
    string? License,
    ProductStatus Status,
    UserSummaryDTO Creator,
    IReadOnlyList<ProductImageDTO> Images,
    double AverageRating,
    int ReviewsCount,
    DateTime CreatedAt);
