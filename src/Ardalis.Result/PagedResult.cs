namespace Ardalis.Result
{
    public class PagedResult<T> : Result<T>
    {
        public PagedResult(PagedInfo pagedInfo, T value) : base(value)
        {
            PagedInfo = pagedInfo;
        }

        public PagedInfo PagedInfo { get; private set; }

        public void SetPagedInfo(PagedInfo pagedInfo)
        {
            PagedInfo = pagedInfo;
        }
    }
}
