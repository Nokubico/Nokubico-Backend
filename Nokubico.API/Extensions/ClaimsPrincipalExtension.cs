using System;
using System.Security.Claims;
using Nokubico.Domain.Enums;

namespace Nokubico.API.Extensions
{
    public static class ClaimsPrincipalExtension
    {
        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            var value = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(value, out var id) ? id : Guid.Empty;
        }

        public static string GetEmail(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;
        }

        public static UserRole GetRole(this ClaimsPrincipal user)
        {
            var value = user.FindFirst(ClaimTypes.Role)?.Value;
            return Enum.TryParse(value, out UserRole role) ? role : UserRole.User;
        }
    }
}