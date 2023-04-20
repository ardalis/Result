using Ardalis.Result.Sample.Core.DTOs;
using Ardalis.Result.Sample.Core.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ardalis.Result.SampleWeb.FunctionalTests;

public class WeatherForecastControllerPost : IClassFixture<WebApplicationFactory<WebMarker>>
{
    private const string CONTROLLER_POST_ROUTE = "/weatherforecast/create";
    private const string ENDPOINT_POST_ROUTE = "/forecast/new";
    private readonly HttpClient _client;

    public WeatherForecastControllerPost(WebApplicationFactory<WebMarker> factory)
    {
        _client = factory.CreateClient();
    }

    [Theory]
    [InlineData(CONTROLLER_POST_ROUTE)]
    [InlineData(ENDPOINT_POST_ROUTE)]
    public async Task ReturnsOkWithValueGivenValidPostalCode(string route)
    {
        var requestDto = new ForecastRequestDto() { PostalCode = "55555" };
        var response = await PostDTOAndGetResponse(requestDto, route);
        response.EnsureSuccessStatusCode();

        var stringResponse = await response.Content.ReadAsStringAsync();
        var forecasts = JsonConvert.DeserializeObject<List<WeatherForecast>>(stringResponse);

        Assert.Equal("Freezing", forecasts.First().Summary);
    }

    [Theory]
    [InlineData(CONTROLLER_POST_ROUTE)]
    [InlineData(ENDPOINT_POST_ROUTE)]
    public async Task ReturnsBadRequestGivenNoPostalCode(string route)
    {
        var requestDto = new ForecastRequestDto() { PostalCode = "" };
        var response = await PostDTOAndGetResponse(requestDto, route);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var stringResponse = await response.Content.ReadAsStringAsync();

        var validationProblemDetails = JsonConvert.DeserializeObject<ValidationProblemDetails>(stringResponse);

        Assert.Contains(validationProblemDetails.Errors, d => d.Key == nameof(ForecastRequestDto.PostalCode));
        Assert.Equal(400, validationProblemDetails.Status);
    }

    [Theory]
    [InlineData(CONTROLLER_POST_ROUTE)]
    [InlineData(ENDPOINT_POST_ROUTE)]
    public async Task ReturnsNotFoundGivenNonExistentPostalCode(string route)
    {
        var requestDto = new ForecastRequestDto() { PostalCode = "NotFound" };
        var response = await PostDTOAndGetResponse(requestDto, route);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var stringResponse = await response.Content.ReadAsStringAsync();

        var problemDetails = JsonConvert.DeserializeObject<ProblemDetails>(stringResponse);

        Assert.Equal("Resource not found.", problemDetails.Title);
        Assert.Equal(404, problemDetails.Status);
    }

    [Theory]
    [InlineData(CONTROLLER_POST_ROUTE)]
    [InlineData(ENDPOINT_POST_ROUTE)]
    public async Task ReturnsBadRequestGivenPostalCodeTooLong(string route)
    {
        var requestDto = new ForecastRequestDto() { PostalCode = "01234567890" };
        var response = await PostDTOAndGetResponse(requestDto, route);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var stringResponse = await response.Content.ReadAsStringAsync();

        var validationProblemDetails = JsonConvert.DeserializeObject<ValidationProblemDetails>(stringResponse);

        Assert.Contains(validationProblemDetails.Errors, d => d.Key == nameof(ForecastRequestDto.PostalCode));
        Assert.Contains(validationProblemDetails.Errors[nameof(ForecastRequestDto.PostalCode)], e => e.Equals("PostalCode cannot exceed 10 characters.", System.StringComparison.OrdinalIgnoreCase));
        Assert.Equal(400, validationProblemDetails.Status);
    }
    
    [Theory]
    [InlineData(CONTROLLER_POST_ROUTE)]
    [InlineData(ENDPOINT_POST_ROUTE)]
    public async Task ReturnsConflictGivenNonExistentPostalCode(string route)
    {
        var requestDto = new ForecastRequestDto() { PostalCode = "Conflict" };
        var response = await PostDTOAndGetResponse(requestDto, route);

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        var stringResponse = await response.Content.ReadAsStringAsync();

        var problemDetails = JsonConvert.DeserializeObject<ProblemDetails>(stringResponse);

        Assert.Equal("There was a conflict.", problemDetails.Title);
        Assert.Equal(409, problemDetails.Status);
    }

    private async Task<HttpResponseMessage> PostDTOAndGetResponse(ForecastRequestDto dto, string route)
    {
        var jsonContent = new StringContent(JsonConvert.SerializeObject(dto),
            Encoding.Default, "application/json");
        return await _client.PostAsync(route, jsonContent);
    }
}
