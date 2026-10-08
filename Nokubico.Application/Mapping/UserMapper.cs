using Nokubico.Application.DTOs;
using Nokubico.Domain.Entities;
using Nokubico.Domain.Enums;

namespace Nokubico.Application.Mapping;

public static class UserMapper
{
    public static UserDTO ToDto(User user)
    {
        ArgumentNullException.ThrowIfNull(user);

        return new UserDTO
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Image = user.Image,
            Bio = user.Bio,
            Location = user.Location,
            Profession = user.Profession,
            Role = user.Role,
            EmailVerified = user.EmailVerified,
            CreatedAt = user.CreatedAt
        };
    }

    // Registration always starts with the ordinary user role. Credentials are
    // handled by the authentication service, never copied onto the user entity.
    public static User ToEntity(RegisterDTO dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var user = new User(dto.Email, dto.Name, UserRole.User);
        return Apply(dto, user);
    }

    // Apply registration profile fields to a user created by authentication.
    // Do not change its identity, email, role, verification or credentials.
    public static User Apply(RegisterDTO dto, User user)
    {
        ArgumentNullException.ThrowIfNull(dto);
        ArgumentNullException.ThrowIfNull(user);

        user.SetName(dto.Name);
        user.SetProfession(dto.Profession);
        return user;
    }

    // Full profile replacement: null optional fields clear the stored values.
    public static User Apply(UserUpdateDTO dto, User user)
    {
        ArgumentNullException.ThrowIfNull(dto);
        ArgumentNullException.ThrowIfNull(user);

        user.SetName(dto.Name);
        user.SetBio(dto.Bio);
        user.SetLocation(dto.Location);
        user.SetProfession(dto.Profession);
        user.SetImage(dto.Image);
        return user;
    }
}
