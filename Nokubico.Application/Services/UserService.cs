using Nokubico.Application.DTOs;
using Nokubico.Application.Interfaces;
using Nokubico.Application.Mapping;
using Nokubico.Domain.Entities;
using Nokubico.Domain.Interface.Auth;

namespace Nokubico.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository), "O repositório de utilizadores não pode ser nulo.");
    }

    public async Task<UserDTO> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("O ID do utilizador não pode ser vazio.", nameof(id));

        var user = await _userRepository.FindById(id, cancellationToken);

        if (user is null)
            throw new InvalidOperationException($"Utilizador com ID {id} não encontrado.");

        return UserMapper.ToDto(user);
    }

    public async Task<UserDTO> GetProfile(Guid userId, CancellationToken cancellationToken = default)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("O ID do utilizador não pode ser vazio.", nameof(userId));

        var user = await _userRepository.FindById(userId, cancellationToken);

        if (user is null)
            throw new InvalidOperationException($"Utilizador com ID {userId} não encontrado.");

        return UserMapper.ToDto(user);
    }

    public async Task<UserDTO> Update(Guid userId, UserUpdateDTO dto, CancellationToken cancellationToken = default)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("O ID do utilizador não pode ser vazio.", nameof(userId));

        ArgumentNullException.ThrowIfNull(dto, nameof(dto));

        var user = await _userRepository.FindById(userId, cancellationToken);

        if (user is null)
            throw new InvalidOperationException($"Utilizador com ID {userId} não encontrado.");

        UserMapper.Apply(dto, user);

        var updatedUser = await _userRepository.Save(user, cancellationToken);

        return UserMapper.ToDto(updatedUser);
    }

    public async Task Delete(Guid userId, CancellationToken cancellationToken = default)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("O ID do utilizador não pode ser vazio.", nameof(userId));

        var user = await _userRepository.FindById(userId, cancellationToken);

        if (user is null)
            throw new InvalidOperationException($"Utilizador com ID {userId} não encontrado.");

        await _userRepository.Delete(user, cancellationToken);
    }
}
