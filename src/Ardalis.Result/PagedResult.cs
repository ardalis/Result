namespace Ardalis.Result
{
    public class PagedResult : Result
    {
        public PagedResult(PagedInfo pagedInfo, ResultStatus status) : base(status)
        {
            PagedInfo = pagedInfo;
        }

        public PagedInfo PagedInfo { get; }
    }
}

