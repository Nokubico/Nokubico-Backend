namespace Nokubico.Application.DTOs;

/// <summary>
/// DTO completo de um post com dados de autor, contadores e flags do utilizador autenticado.
/// </summary>
public record PostDTO(
    Guid Id,
    string? Content,
    string? ImageUrl,
    string? VideoUrl,
    DateTime CreatedAt,
    UserSummaryDTO Author,
    int LikesCount,
    int CommentsCount,
    int SharesCount,
    bool LikedByMe,
    bool BookmarkedByMe);
