namespace Nokubico.Application.DTOs;

/// <summary>
/// DTO com a informação completa de um comentário.
/// </summary>
public record CommentDTO(
    Guid Id,
    string Content,
    DateTime CreatedAt,
    UserSummaryDTO Author);
