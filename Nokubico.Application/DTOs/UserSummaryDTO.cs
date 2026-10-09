namespace Nokubico.Application.DTOs;

/// <summary>
/// DTO resumido de um utilizador para uso em contextos onde não deve expor dados sensíveis.
/// Utilizado como autor em posts e comentários.
/// </summary>
public record UserSummaryDTO(
    Guid Id,
    string Name,
    string? Image,
    string? Profession);
