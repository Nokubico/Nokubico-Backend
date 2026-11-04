using Nokubico.Application.DTOs;
using Nokubico.Domain.Entities;

namespace Nokubico.Application.Mapping;

public static class ReviewMapper
{
    public static ReviewDTO ToDto(Review review)
    {
        ArgumentNullException.ThrowIfNull(review);

        return new ReviewDTO(
            Id: review.Id,
            Rating: review.Rating,
            Comment: review.Comment,
            CreatedAt: review.CreatedAt,
            Author: UserMapper.ToSummaryDto(
                review.User ?? throw new InvalidOperationException("A avaliação não tem autor associado.")));
    }

    public static Review ToEntity(CreateReviewDTO dto, Guid productId, Guid userId)
    {
        ArgumentNullException.ThrowIfNull(dto);

        if (productId == Guid.Empty)
            throw new ArgumentException("O ID do produto não pode ser vazio.", nameof(productId));

        if (userId == Guid.Empty)
            throw new ArgumentException("O ID do utilizador não pode ser vazio.", nameof(userId));

        dto.Validate();
        return new Review(productId, userId, dto.Rating, dto.Comment);
    }
}
