using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Nokubico.Domain.Pagination;
using Nokubico.Infra.Data.Context;

namespace Nokubico.Infra.Data.Repository
{
    public abstract class BaseRepository
    {
        protected readonly AppDbContext Context;

        protected BaseRepository(AppDbContext context)
        {
            Context = context;
        }

        protected PagedList<T> ToPagedList<T>(List<T> items, long totalItems, PaginationParams pagination)
        {
            return new PagedList<T>(items, pagination.Page, pagination.PageSize, totalItems);
        }
    }
}