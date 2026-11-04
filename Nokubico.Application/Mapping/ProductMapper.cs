using Nokubico.Application.DTOs;
using Nokubico.Domain.Entities;

namespace Nokubico.Application.Mapping;

public static class ProductMapper
{
    public static ProductDTO ToDto(
        Product product,
        double? averageRating = null,
        int? reviewsCount = null)
    {
        ArgumentNullException.ThrowIfNull(product);

        if (reviewsCount < 0)
            throw new ArgumentOutOfRangeException(nameof(reviewsCount));

        var reviews = product.Reviews;
        var reviewCount = reviewsCount ?? reviews.Count;
        var rating = averageRating
            ?? (reviews.Count == 0 ? 0 : reviews.Average(review => review.Rating));

        return new ProductDTO(
            Id: product.Id,
            Title: product.Title,
            Slug: product.Slug,
            Description: product.Description,
            Category: product.Category,
            Price: product.Price,
            Currency: product.Currency,
            ThumbnailUrl: product.Thumbnail,
            License: product.License,
            Status: product.Status,
            Creator: UserMapper.ToSummaryDto(
                product.Creator ?? throw new InvalidOperationException("O produto não tem criador associado.")),
            Images: product.Images
                .OrderBy(image => image.Position)
                .Select(image => new ProductImageDTO(image.ImageUrl, image.Position))
                .ToArray(),
            AverageRating: rating,
            ReviewsCount: reviewCount,
            CreatedAt: product.CreatedAt);
    }

    public static Product ToEntity(CreateProductDTO dto, Guid creatorId)
    {
        ArgumentNullException.ThrowIfNull(dto);

        if (creatorId == Guid.Empty)
            throw new ArgumentException("O ID do criador não pode ser vazio.", nameof(creatorId));

        dto.Validate();

        var product = new Product(
            dto.Title,
            dto.Slug,
            dto.Category,
            dto.Price,
            dto.Currency,
            creatorId);

        product.SetDetails(dto.Slug, dto.Description, dto.Category, dto.License, dto.ThumbnailUrl);
        product.ReplaceImages(dto.ImageUrls);
        return product;
    }

    public static void ApplyUpdate(UpdateProductDTO dto, Product product)
    {
        ArgumentNullException.ThrowIfNull(dto);
        ArgumentNullException.ThrowIfNull(product);

        dto.Validate();

        product.SetTitle(dto.Title);
        product.SetPrice(dto.Price);
        product.SetCurrency(dto.Currency);
        product.SetDetails(product.Slug, dto.Description, dto.Category, dto.License, dto.ThumbnailUrl);
        product.ReplaceImages(dto.ImageUrls);
    }
}
