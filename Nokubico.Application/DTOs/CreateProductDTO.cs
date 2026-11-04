using System.ComponentModel.DataAnnotations;

namespace Nokubico.Application.DTOs;

public record CreateProductDTO(
    [property: Required] string Title,
    [property: Required] string Category,
    [property: Range(0, long.MaxValue)] long Price,
    [property: Required, StringLength(3, MinimumLength = 3)] string Currency,
    [property: Required] string License,
    [property: Required] string ThumbnailUrl,
    [property: Required] IReadOnlyList<string> ImageUrls,
    string? Slug = null,
    string? Description = null)
{
    public void Validate()
    {
        ProductDtoValidation.Validate(
            Title, Category, Price, Currency, License, ThumbnailUrl, ImageUrls);
    }
}
