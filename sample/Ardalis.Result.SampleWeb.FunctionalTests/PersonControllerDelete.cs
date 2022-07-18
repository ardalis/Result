using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Ardalis.Result.SampleWeb.FunctionalTests;

public class PersonControllerDelete : IClassFixture<WebApplicationFactory<Startup>>
{
    private const string MEDIATR_CONTROLLER_POST_ROUTE = "/mediatr/person/remove/{0}";
    private const string CONTROLLER_POST_ROUTE = "/person/remove/{0}";
    private const string ENDPOINT_POST_ROUTE = "/person/delete/{0}";

    private readonly HttpClient _client;

    public PersonControllerDelete(WebApplicationFactory<Startup> factory)
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
        Assert.Contains("Person Not Found", stringResponse);
    }

    private async Task<HttpResponseMessage> SendDeleteRequest(string route, int id)
    {
        return await _client.DeleteAsync(string.Format(route, id));
    }
}
