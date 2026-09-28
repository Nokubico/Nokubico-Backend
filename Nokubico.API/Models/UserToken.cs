using Nokubico.Domain.Enums;

namespace Nokubico.API.Models
{
    public class UserToken
    {
        public string Token { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public string Email { get; set; } = string.Empty;
    }
}