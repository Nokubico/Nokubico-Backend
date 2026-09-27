using System;
using Nokubico.Domain.Entities;
using Nokubico.Domain.Pagination;

namespace Nokubico.Domain.Interface.Auth
{
    public interface IUserRepository
    {
        User? FindByEmail(string email);

        User? FindById(Guid id);

        User Save(User user);

        void Delete(User user);

        PagedList<User> FindAll(PaginationParams pagination);
    }
}