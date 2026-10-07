namespace Nokubico.Application.DTOs;

public record UserUpdateDTO(
    string Name,
    string? Image,
    string? Bio,
    string? Location,
    string? Profession);