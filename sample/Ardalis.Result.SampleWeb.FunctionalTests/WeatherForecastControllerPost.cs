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
        private readonly HttpClient _client;

        public WeatherForecastControllerPost(WebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task ReturnsOkWithValueGivenValidPostalCode()
        {
            var requestDto = new ForecastRequestDto() { PostalCode = "55555" };
            var jsonContent = new StringContent(JsonConvert.SerializeObject(requestDto), Encoding.Default, "application/json");
            var response = await _client.PostAsync("/weatherforecast", jsonContent);
            response.EnsureSuccessStatusCode();

            var stringResponse = await response.Content.ReadAsStringAsync();
            var forecasts = JsonConvert.DeserializeObject<List<WeatherForecast>>(stringResponse);

            Assert.Equal("Freezing", forecasts.First().Summary);
        }

        [Fact]
        public async Task ReturnsBadRequestGivenNoPostalCode()
        {
            var requestDto = new ForecastRequestDto() { PostalCode = "" };
            var jsonContent = new StringContent(JsonConvert.SerializeObject(requestDto), Encoding.Default, "application/json");
            var response = await _client.PostAsync("/weatherforecast", jsonContent);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task ReturnsNotFoundGivenNonExistentPostalCode()
        {
            var requestDto = new ForecastRequestDto() { PostalCode = "NotFound" };
            var jsonContent = new StringContent(JsonConvert.SerializeObject(requestDto), Encoding.Default, "application/json");
            var response = await _client.PostAsync("/weatherforecast", jsonContent);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task ReturnsBadRequestGivenPostalCodeTooLong()
        {
            var requestDto = new ForecastRequestDto() { PostalCode = "01234567890" };
            var jsonContent = new StringContent(JsonConvert.SerializeObject(requestDto), Encoding.Default, "application/json");
            var response = await _client.PostAsync("/weatherforecast", jsonContent);
            //response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var stringResponse = await response.Content.ReadAsStringAsync();
            Assert.Contains("PostalCode cannot exceed 10 characters.", stringResponse);
        }
    }
}
