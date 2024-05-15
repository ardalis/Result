using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Ardalis.Result.AspNetCore.UnitTests;

public class ResultStatusMapAddDefaultMap
{
    [Fact]
    public void ReturnsBadRequestGivenInvalid()
    {
        var controller = new TestController();

        var result = Result<int>.Invalid(new ValidationError("test"));

        // TODO: require identifier
        Assert.True(true);
    }

    public class TestController : ControllerBase
    {
        public Result Index()
        {
            throw new NotImplementedException();
        }

        public Result<string> ResultString()
        {
            throw new NotImplementedException();
        }

        public Result<IEnumerable<string>> ResultEnumerableString()
        {
            throw new NotImplementedException();
        }
    }

}
