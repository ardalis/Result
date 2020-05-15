using Ardalis.Sample.Core.DTOs;
using Ardalis.Sample.Core.Model;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ardalis.Result.SampleWeb.FunctionalTests
{
    public class WeatherForecastControllerPost : IClassFixture<WebApplicationFactory<Startup>>
    {
        private const string CONTROLLER_POST_ROUTE = "/weatherforecast/create";
        private const string ENDPOINT_POST_ROUTE = "/forecast/new";
        private readonly HttpClient _client;

        public WeatherForecastControllerPost(WebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Theory]
        [InlineData(CONTROLLER_POST_ROUTE)]
        [InlineData(ENDPOINT_POST_ROUTE)]
        public async Task ReturnsOkWithValueGivenValidPostalCode(string route)
        {
            var requestDto = new ForecastRequestDto() { PostalCode = "55555" };
            var jsonContent = new StringContent(JsonConvert.SerializeObject(requestDto), Encoding.Default, "application/json");
            var response = await _client.PostAsync(route, jsonContent);
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
            var jsonContent = new StringContent(JsonConvert.SerializeObject(requestDto), Encoding.Default, "application/json");
            var response = await _client.PostAsync(route, jsonContent);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Theory]
        [InlineData(CONTROLLER_POST_ROUTE)]
        [InlineData(ENDPOINT_POST_ROUTE)]
        public async Task ReturnsNotFoundGivenNonExistentPostalCode(string route)
        {
            var requestDto = new ForecastRequestDto() { PostalCode = "NotFound" };
            var jsonContent = new StringContent(JsonConvert.SerializeObject(requestDto), Encoding.Default, "application/json");
            var response = await _client.PostAsync(route, jsonContent);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Theory]
        [InlineData(CONTROLLER_POST_ROUTE)]
        [InlineData(ENDPOINT_POST_ROUTE)]
        public async Task ReturnsBadRequestGivenPostalCodeTooLong(string route)
        {
            var requestDto = new ForecastRequestDto() { PostalCode = "01234567890" };
            var jsonContent = new StringContent(JsonConvert.SerializeObject(requestDto), Encoding.Default, "application/json");
            var response = await _client.PostAsync(route, jsonContent);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var stringResponse = await response.Content.ReadAsStringAsync();
            Assert.Contains("PostalCode cannot exceed 10 characters.", stringResponse);
        }
    }
}
