using System.Text.Json.Serialization;

namespace Ardalis.Result
{
    public class PagedInfo(long pageNumber, long pageSize, long totalPages, long totalRecords)
    {
        [JsonInclude]
        public long PageNumber { get; private set; } = pageNumber;
        [JsonInclude]
        public long PageSize { get; private set; } = pageSize;
        [JsonInclude]
        public long TotalPages { get; private set; } = totalPages;
        [JsonInclude]
        public long TotalRecords { get; private set; } = totalRecords;

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
