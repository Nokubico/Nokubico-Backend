namespace Nokubico.Application.DTOs;

/// <summary>
/// DTO para criar um novo post. Requer pelo menos um campo de conteúdo não vazio.
/// </summary>
public record CreatePostDTO(
    string? Content,
    string? ImageUrl,
    string? VideoUrl)
{
    public CreatePostDTO() : this(null, null, null)
    {
    }

    /// <summary>
    /// Valida que pelo menos um campo de conteúdo (Content, ImageUrl ou VideoUrl) foi fornecido.
    /// </summary>
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Content) && 
            string.IsNullOrWhiteSpace(ImageUrl) && 
            string.IsNullOrWhiteSpace(VideoUrl))
        {
            throw new ArgumentException(
                "O post deve ter pelo menos um do seguinte: conteúdo, imagem ou vídeo.");
        }
    }
}
