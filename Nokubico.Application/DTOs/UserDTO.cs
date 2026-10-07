using Nokubico.Domain.Enums;

namespace Nokubico.Application.DTOs;

public record UserDTO(
    Guid Id,
    string Email,
    string Name,
    string? Image,
    string? Bio,
    string? Location,
    string? Profession,
    UserRole Role,
    bool EmailVerified,
    DateTime CreatedAt);