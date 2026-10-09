namespace Nokubico.Application.DTOs;

/// <summary>
/// DTO para atualizar um post existente.
/// </summary>
public record UpdatePostDTO(
    string? Content,
    string? ImageUrl,
    string? VideoUrl);
