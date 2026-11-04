using System.ComponentModel.DataAnnotations;

namespace Nokubico.Application.DTOs;

public record CreateReviewDTO(
    [property: Range(1, 5)] int Rating,
    string? Comment = null)
{
    public void Validate()
    {
        if (Rating is < 1 or > 5)
        {
            throw new ArgumentOutOfRangeException(
                nameof(Rating), Rating, "A avaliação deve estar entre 1 e 5.");
        }
    }
}
