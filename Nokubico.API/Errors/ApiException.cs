namespace Nokubico.API.Errors;

/// <summary>
/// Exceção da API com mapeamento para código HTTP.
/// </summary>
public class ApiException : Exception
{
    public int HttpStatusCode { get; set; }

    public ApiException(string message, int httpStatusCode = 500) : base(message)
    {
        HttpStatusCode = httpStatusCode;
    }

    public ApiException(string message, int httpStatusCode, Exception innerException)
        : base(message, innerException)
    {
        HttpStatusCode = httpStatusCode;
    }
}
