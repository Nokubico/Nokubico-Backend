using Nokubico.Domain.Enums;

namespace Nokubico.Application.DTOs;

public class UserDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Image { get; set; }
    public string? Bio { get; set; }
    public string? Location { get; set; }
    public string? Profession { get; set; }
    public UserRole Role { get; set; }
    public bool EmailVerified { get; set; }
    public DateTime CreatedAt { get; set; }
}
