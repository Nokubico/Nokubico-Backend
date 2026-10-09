namespace Nokubico.Application.DTOs;

/// <summary>
/// DTO simples de um bookmark de um post.
/// </summary>
public record BookmarkDTO(
    Guid UserId,
    Guid PostId,
    DateTime CreatedAt);
