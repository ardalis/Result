namespace Ardalis.Result
{
    public class PagedInfo
    {

        public PagedInfo(long pageNumber, long pageSize, long totalPages, long totalRecords)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalPages = totalPages;
            TotalRecords = totalRecords;
        }

        public long PageNumber { get; private set; }
        public long PageSize { get; private set; }
        public long TotalPages { get; private set; }
        public long TotalRecords { get; private set; }

        public PagedInfo SetPageNumber(long pageNumber)
        {
            PageNumber = pageNumber;

            return this;
        }

        public PagedInfo SetPageSize(long pageSize)
        {
            PageSize = pageSize;

            return this;
        }

        public PagedInfo SetTotalPages(long totalPages)
        {
            TotalPages = totalPages;

            return this;
        }

        public PagedInfo SetTotalRecords(long totalRecords)
        {
            TotalRecords = totalRecords;

            return this;
        }
    }
}
