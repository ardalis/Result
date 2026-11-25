using Ardalis.Result.Sample.Core.DTOs;
using Ardalis.Result.Sample.Core.Model;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ardalis.Result.SampleMinimalApi.FunctionalTests;
public class NewWeatherForecast : IClassFixture<WebApplicationFactory<IWebMarker>>
{
    private const string ENDPOINT_POST_ROUTE = "/forecast/new";
    private readonly HttpClient _client;

    public NewWeatherForecast(WebApplicationFactory<IWebMarker> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task ReturnsOkWithValueGivenValidPostalCode()
    {
        var requestDto = new ForecastRequestDto() { PostalCode = "55555" };
        
        var jsonContent = new StringContent(JsonSerializer.Serialize(requestDto), Encoding.Default, "application/json");
        var response = await _client.PostAsync(ENDPOINT_POST_ROUTE, jsonContent);
        response.EnsureSuccessStatusCode();

        var stringResponse = await response.Content.ReadAsStringAsync();
        var forecasts = JsonSerializer.Deserialize<List<WeatherForecast>>(stringResponse, JsonSerializerOptions.Web);

        Assert.Equal("Freezing", forecasts?.First()?.Summary);
    }

    [Fact]
    public async Task ReturnsBadRequestGivenNoPostalCode()
    {
        var requestDto = new ForecastRequestDto() { PostalCode = "" };
        var jsonContent = new StringContent(JsonSerializer.Serialize(requestDto), Encoding.Default, "application/json");
        var response = await _client.PostAsync(ENDPOINT_POST_ROUTE, jsonContent);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task ReturnsNotFoundGivenNonExistentPostalCode()
    {
        var requestDto = new ForecastRequestDto() { PostalCode = "NotFound" };
        var jsonContent = new StringContent(JsonSerializer.Serialize(requestDto), Encoding.Default, "application/json");
        var response = await _client.PostAsync(ENDPOINT_POST_ROUTE, jsonContent);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task ReturnsBadRequestGivenPostalCodeTooLong()
    {
        var requestDto = new ForecastRequestDto() { PostalCode = "01234567890" };
        var jsonContent = new StringContent(JsonSerializer.Serialize(requestDto), Encoding.Default, "application/json");
        var response = await _client.PostAsync(ENDPOINT_POST_ROUTE, jsonContent);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var stringResponse = await response.Content.ReadAsStringAsync();
        Assert.Contains("PostalCode cannot exceed 10 characters.", stringResponse);
    }
    
    [Fact]
    public async Task ReturnsConflictGivenNonExistentPostalCode()
    {
        var requestDto = new ForecastRequestDto() { PostalCode = "Conflict" };
        var jsonContent = new StringContent(JsonSerializer.Serialize(requestDto), Encoding.Default, "application/json");
        var response = await _client.PostAsync(ENDPOINT_POST_ROUTE, jsonContent);

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }
}