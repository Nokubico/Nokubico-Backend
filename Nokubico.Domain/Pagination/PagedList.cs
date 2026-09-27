using System.Collections.Generic;

namespace Nokubico.Domain.Pagination
{
    public class PagedList<T>
    {
        public List<T> Items { get; private set; }
        public int Page { get; private set; }
        public int PageSize { get; private set; }
        public long TotalItems { get; private set; }
        public int TotalPages { get; private set; }

        public PagedList(List<T> items, int page, int pageSize, long totalItems)
        {
            Items = items;
            Page = page;
            PageSize = pageSize;
            TotalItems = totalItems;
            TotalPages = pageSize > 0 ? (int)Math.Ceiling((double)totalItems / pageSize) : 0;
        }
    }
}