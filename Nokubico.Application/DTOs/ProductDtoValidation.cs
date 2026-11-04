using Nokubico.Domain.Validation;

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
            throw new DomainException("O título do produto é obrigatório.");

        if (string.IsNullOrWhiteSpace(category))
            throw new DomainException("A categoria do produto é obrigatória.");

        if (price < 0)
            throw new DomainException("O preço do produto não pode ser negativo.");

        if (string.IsNullOrWhiteSpace(currency) || currency.Length != 3)
            throw new DomainException("A moeda deve conter exatamente 3 caracteres.");

        if (string.IsNullOrWhiteSpace(license))
            throw new DomainException("A licença do produto é obrigatória.");

        if (string.IsNullOrWhiteSpace(thumbnailUrl))
            throw new DomainException("A imagem de capa do produto é obrigatória.");

        if (imageUrls is null)
            throw new DomainException("A lista de imagens é obrigatória.");

        if (imageUrls.Count > Nokubico.Domain.Entities.Product.MaximumImages)
            throw new DomainException(
                $"Um produto pode ter no máximo {Nokubico.Domain.Entities.Product.MaximumImages} imagens.");

        if (imageUrls.Any(string.IsNullOrWhiteSpace))
            throw new DomainException("Os URLs das imagens não podem estar vazios.");

        if (imageUrls.Distinct(StringComparer.Ordinal).Count() != imageUrls.Count)
            throw new DomainException("Os URLs das imagens não podem repetir-se.");
    }
}
