namespace Ardalis.Result
{
    public class PagedResult<T> : Result<T>
    {
        public PagedResult(PagedInfo pagedInfo, T value) : base(value)
        {
            PagedInfo = pagedInfo;
        }

        public PagedInfo PagedInfo { get; }
    }

    public class PagedResult : Result
    {
        public PagedResult(PagedInfo pagedInfo, ResultStatus status) : base(status)
        {
            PagedInfo = pagedInfo;
        }

        public PagedInfo PagedInfo { get; }
    }
}

