using FluentAssertions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Ardalis.Result.UnitTests
{
    public class ResultVoidMapAsync
    {
        [Fact]
        public async Task ShouldProduceSuccessWithValueGivenResultOfVoidAsync()
        {
            var result = Result.Success();

            var actual = await result.MapAsync(async _ =>
            {
                await Task.Delay(1); // Simulate async operation
                return "Success";
            });

            actual.Value.Should().Be("Success");
            actual.Status.Should().Be(ResultStatus.Ok);
        }

        [Fact]
        public async Task ShouldProduceNotFoundAsync()
        {
            var result = Result.NotFound();

            var actual = await result.MapAsync(async _ =>
            {
                await Task.Delay(1); // This should not be called
                return "This should be ignored";
            });

            actual.Status.Should().Be(ResultStatus.NotFound);
            actual.Value.Should().BeNull();
        }

        [Fact]
        public async Task ShouldProduceUnauthorizedAsync()
        {
            var result = Result.Unauthorized();

            var actual = await result.MapAsync(async _ =>
            {
                await Task.Delay(1); // This should not be called
                return "This should be ignored";
            });

            actual.Status.Should().Be(ResultStatus.Unauthorized);
            actual.Value.Should().BeNull();
        }

        [Fact]
        public async Task ShouldProduceForbiddenAsync()
        {
            var result = Result.Forbidden();

            var actual = await result.MapAsync(async _ =>
            {
                await Task.Delay(1); // This should not be called
                return "This should be ignored";
            });

            actual.Status.Should().Be(ResultStatus.Forbidden);
            actual.Value.Should().BeNull();
        }

        [Fact]
        public async Task ShouldProduceInvalidWithEmptyListAsync()
        {
            var result = Result.Invalid(new List<ValidationError>());

            var actual = await result.MapAsync(async _ =>
            {
                await Task.Delay(1); // This should not be called
                return "This should be ignored";
            });

            actual.Status.Should().Be(ResultStatus.Invalid);
            actual.ValidationErrors.Should().BeEmpty();
            actual.Value.Should().BeNull();
        }

        [Fact]
        public async Task ShouldProduceInvalidAsync()
        {
            var validationError = new ValidationError { ErrorMessage = "Invalid input" };
            var result = Result.Invalid(validationError);

            var actual = await result.MapAsync(async _ =>
            {
                await Task.Delay(1); // This should not be called
                return "This should be ignored";
            });

            actual.Status.Should().Be(ResultStatus.Invalid);
            actual.ValidationErrors.Should().ContainSingle().Which.Should().BeEquivalentTo(validationError);
            actual.Value.Should().BeNull();
        }

        [Fact]
        public async Task ShouldProduceErrorAsync()
        {
            var result = Result.Error();

            var actual = await result.MapAsync(async _ =>
            {
                await Task.Delay(1); // This should not be called
                return "This should be ignored";
            });

            actual.Status.Should().Be(ResultStatus.Error);
            actual.Value.Should().BeNull();
        }

        [Fact]
        public async Task ShouldProduceConflictAsync()
        {
            var result = Result.Conflict();

            var actual = await result.MapAsync(async _ =>
            {
                await Task.Delay(1); // This should not be called
                return "This should be ignored";
            });

            actual.Status.Should().Be(ResultStatus.Conflict);
            actual.Value.Should().BeNull();
        }

        [Fact]
        public async Task ShouldProduceUnavailableAsync()
        {
            var result = Result.Unavailable();

            var actual = await result.MapAsync(async _ =>
            {
                await Task.Delay(1); // This should not be called
                return "This should be ignored";
            });

            actual.Status.Should().Be(ResultStatus.Unavailable);
            actual.Value.Should().BeNull();
        }

        [Fact]
        public async Task ShouldProduceCriticalErrorAsync()
        {
            var result = Result.CriticalError();

            var actual = await result.MapAsync(async _ =>
            {
                await Task.Delay(1); // This should not be called
                return "This should be ignored";
            });

            actual.Status.Should().Be(ResultStatus.CriticalError);
            actual.Value.Should().BeNull();
        }
    }
}
