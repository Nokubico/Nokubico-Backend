using System;
using Nokubico.Domain.Entities;

namespace Nokubico.Domain.Interface.Auth
{
    public interface ISessionRepository
    {
        Session? FindActiveByToken(string token);

        Session Save(Session session);

        void Delete(Session session);

        void DeleteAllForUser(Guid userId);

        long DeleteExpired(DateTime now);
    }
}