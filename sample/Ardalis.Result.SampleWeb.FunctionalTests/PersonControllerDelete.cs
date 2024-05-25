using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result.Sample.Core.DTOs;
using Xunit;

namespace Ardalis.Result.SampleWeb.FunctionalTests;

public class PersonControllerDelete : IClassFixture<WebApplicationFactory<WebMarker>>
{
    private const string MEDIATR_CONTROLLER_POST_ROUTE = "/mediatr/person/remove/{0}";
    private const string CONTROLLER_POST_ROUTE = "/person/remove/{0}";
    private const string ENDPOINT_POST_ROUTE = "/person/delete/{0}";

    private readonly HttpClient _client;

    public PersonControllerDelete(WebApplicationFactory<WebMarker> factory)
    {
        _client = factory.CreateClient();
    }

    [Theory]
    [InlineData(MEDIATR_CONTROLLER_POST_ROUTE)]
    [InlineData(CONTROLLER_POST_ROUTE)]
    [InlineData(ENDPOINT_POST_ROUTE)]
    public async Task ReturnsOkWithoutValueGivenKnownId(string route)
    {
        var response = await SendDeleteRequest(route, 1);
        response.EnsureSuccessStatusCode();

        var stringResponse = await response.Content.ReadAsStringAsync();

        Assert.Equal(string.Empty, stringResponse);
    }

    [Theory]
    [InlineData(MEDIATR_CONTROLLER_POST_ROUTE)]
    [InlineData(CONTROLLER_POST_ROUTE)]
    [InlineData(ENDPOINT_POST_ROUTE)]
    public async Task ReturnsNotFoundGivenUnknownId(string route)
    {
        var response = await SendDeleteRequest(route, 2);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var stringResponse = await response.Content.ReadAsStringAsync();

        var problemDetails = JsonConvert.DeserializeObject<ProblemDetails>(stringResponse);

        Assert.Contains("Resource not found.", problemDetails.Title);
        Assert.Contains("Person with id 2 Not Found", problemDetails.Detail);
        Assert.Equal(404, problemDetails.Status);
    }

    private async Task<HttpResponseMessage> SendDeleteRequest(string route, int id)
    {
        return await _client.DeleteAsync(string.Format(route, id));
    }
}
