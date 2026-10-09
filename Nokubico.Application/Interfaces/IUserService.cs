using Nokubico.Application.DTOs;

namespace Nokubico.Application.Interfaces;

public interface IUserService
{
    Task<UserDTO> GetById(Guid id, CancellationToken cancellationToken = default);

    Task<UserDTO> GetProfile(Guid userId, CancellationToken cancellationToken = default);

    Task<UserDTO> Update(Guid userId, UserUpdateDTO dto, CancellationToken cancellationToken = default);

    Task Delete(Guid userId, CancellationToken cancellationToken = default);
}
