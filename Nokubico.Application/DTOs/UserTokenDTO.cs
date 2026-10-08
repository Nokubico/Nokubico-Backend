using Nokubico.Domain.Enums;

namespace Nokubico.Application.DTOs;

public class UserTokenDTO
{
    public string Token { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public UserRole Role { get; set; }
}
