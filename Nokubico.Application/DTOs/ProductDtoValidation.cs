namespace Nokubico.Application.DTOs;

internal static class ProductDtoValidation
{
    public static void Validate(
        string title,
        string category,
        long price,
        string currency,
        string license,
        string thumbnailUrl,
        IReadOnlyList<string> imageUrls)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("O título do produto é obrigatório.", nameof(title));

        if (string.IsNullOrWhiteSpace(category))
            throw new ArgumentException("A categoria do produto é obrigatória.", nameof(category));

        if (price < 0)
            throw new ArgumentOutOfRangeException(nameof(price), price, "O preço não pode ser negativo.");

        if (string.IsNullOrWhiteSpace(currency) || currency.Length != 3)
            throw new ArgumentException("A moeda deve conter exatamente 3 caracteres.", nameof(currency));

        if (string.IsNullOrWhiteSpace(license))
            throw new ArgumentException("A licença do produto é obrigatória.", nameof(license));

        if (string.IsNullOrWhiteSpace(thumbnailUrl))
            throw new ArgumentException("A imagem de capa do produto é obrigatória.", nameof(thumbnailUrl));

        ArgumentNullException.ThrowIfNull(imageUrls);

        if (imageUrls.Count > short.MaxValue + 1)
            throw new ArgumentOutOfRangeException(nameof(imageUrls), "O produto excede o limite de imagens.");

        if (imageUrls.Any(string.IsNullOrWhiteSpace))
            throw new ArgumentException("Os URLs das imagens não podem estar vazios.", nameof(imageUrls));

        if (imageUrls.Distinct(StringComparer.Ordinal).Count() != imageUrls.Count)
            throw new ArgumentException("Os URLs das imagens não podem repetir-se.", nameof(imageUrls));
    }
}
