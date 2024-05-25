namespace Ardalis.Result.Sample.Core.Services;

public class BadResultService
{
    public Result ReturnErrorWithMessage(string message)
    {
        var result = Result.Error(message);
        return result;
    }

}
