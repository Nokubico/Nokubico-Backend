namespace Nokubico.Application.DTOs;

/// <summary>
/// DTO simples de um like em um post.
/// </summary>
public record LikeDTO(
    Guid UserId,
    Guid PostId,
    DateTime CreatedAt);
