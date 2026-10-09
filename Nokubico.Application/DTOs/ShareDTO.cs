namespace Nokubico.Application.DTOs;

/// <summary>
/// DTO simples de um share de um post.
/// </summary>
public record ShareDTO(
    Guid UserId,
    Guid PostId,
    DateTime CreatedAt);
