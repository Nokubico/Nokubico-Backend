using System;
using Nokubico.Domain.Entities;

namespace Nokubico.Domain.Interface.Auth
{
    public interface ISessionRepository
    {
        Task<Session?> FindActiveByToken(string token, CancellationToken cancellationToken = default);

        Task<Session> Save(Session session, CancellationToken cancellationToken = default);

        Task Delete(Session session, CancellationToken cancellationToken = default);

        Task DeleteAllForUser(Guid userId, CancellationToken cancellationToken = default);

        Task<long> DeleteExpired(DateTime now, CancellationToken cancellationToken = default);
    }
}