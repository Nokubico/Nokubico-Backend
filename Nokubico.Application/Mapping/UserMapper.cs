using Nokubico.Application.DTOs;
using Nokubico.Domain.Entities;

namespace Nokubico.Application.Mapping;

public static class UserMapper
{
    public static UserDTO ToDto(User user)
    {
        ArgumentNullException.ThrowIfNull(user);

        return new UserDTO(
            user.Id,
            user.Email,
            user.Name,
            user.Image,
            user.Bio,
            user.Location,
            user.Profession,
            user.Role,
            user.EmailVerified,
            user.CreatedAt);
    }

    public static UserSummaryDTO ToSummaryDto(User user)
    {
        ArgumentNullException.ThrowIfNull(user);

        return new UserSummaryDTO(
            user.Id,
            user.Name,
            user.Image,
            user.Profession);
    }

    public static User Apply(UserUpdateDTO dto, User user)
    {
        ArgumentNullException.ThrowIfNull(dto);
        ArgumentNullException.ThrowIfNull(user);

        user.SetName(dto.Name);
        user.SetImage(dto.Image);
        user.SetBio(dto.Bio);
        user.SetLocation(dto.Location);
        user.SetProfession(dto.Profession);

        return user;
    }
}   