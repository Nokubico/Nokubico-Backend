namespace Nokubico.Application.DTOs;

public record ReviewDTO(
    Guid Id,
    int Rating,
    string? Comment,
    DateTime CreatedAt,
    UserSummaryDTO Author);
