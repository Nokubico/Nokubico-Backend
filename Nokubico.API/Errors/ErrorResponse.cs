namespace Nokubico.API.Errors;

/// <summary>
/// Modelo padronizado de resposta de erro da API.
/// </summary>
public record ErrorResponse(
    int Status,
    string Message,
    IDictionary<string, string[]>? Errors = null);
