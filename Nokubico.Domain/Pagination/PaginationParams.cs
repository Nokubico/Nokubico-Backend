namespace Nokubico.Domain.Pagination
{
    public class PaginationParams
    {
        public int Page { get; private set; }
        public int PageSize { get; private set; }

        public PaginationParams(int page = 1, int pageSize = 20)
        {
            Page = page > 0 ? page : 1;
            PageSize = pageSize > 0 ? pageSize : 20;
        }

        public int Offset
        {
            get
            {
                return (Page - 1) * PageSize;
            }
        }
    }
}