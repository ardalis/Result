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

public class PersonControllerCreate : IClassFixture<WebApplicationFactory<WebMarker>>
{
    private const string MEDIATR_CONTROLLER_POST_ROUTE = "/mediatr/person/create/";
    private const string CONTROLLER_POST_ROUTE = "/person/new/";
    private const string ENDPOINT_POST_ROUTE = "/person/create/";

    private readonly HttpClient _client;

    public PersonControllerCreate(WebApplicationFactory<WebMarker> factory)
    {
        _client = factory.CreateClient();
    }

    [Theory]
    [InlineData(MEDIATR_CONTROLLER_POST_ROUTE)]
    [InlineData(CONTROLLER_POST_ROUTE)]
    [InlineData(ENDPOINT_POST_ROUTE)]
    public async Task ReturnsConflictGivenExistPerson(string route)
    {
        var createPersonRequestDto = new CreatePersonRequestDto 
        { 
            FirstName = "John",
            LastName = "Smith"
        };
        var json = JsonConvert.SerializeObject(createPersonRequestDto);
        var response = await _client.PostAsync(route, new StringContent(json, Encoding.UTF8, "application/json"));

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        var stringResponse = await response.Content.ReadAsStringAsync();

        var problemDetails = JsonConvert.DeserializeObject<ProblemDetails>(stringResponse);

        Assert.Contains("There was a conflict.", problemDetails.Title);
        Assert.Contains("Next error(s) occurred:* Person (John Smith) is exist", problemDetails.Detail);
        Assert.Equal(409, problemDetails.Status);
    }
}
