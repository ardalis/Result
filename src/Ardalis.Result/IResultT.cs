namespace Ardalis.Result
{
    public interface IResult<T> : IResult
    {
        T Value { get; }
    }
}
