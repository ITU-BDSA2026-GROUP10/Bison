using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Bison.Razor.tests;

/*test idea: 
    assert wether giving argument page nr 0 and 1 returns the same task list 
    in observationservice.getobservations(int) and GetObservationsFromAuthor(int)
    something similar can be done for the other methods if stm corrections
    the sut is observationservice.
    */

public class IntegrationTestsBasic  : IClassFixture<WebApplicationFactory<Program>>
{
    //See: https://github.com/dotnet/AspNetCore.Docs.Samples/blob/main/test/integration-tests/10.x/IntegrationTestsSample/tests/RazorPagesProject.Tests/IntegrationTests/BasicTests.cs
    //From: https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-10.0&pivots=xunit

    private readonly WebApplicationFactory<Program> _factory;

    public IntegrationTestsBasic(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Theory]
    [InlineData("/")]
    [InlineData("/obs")]
    [InlineData("/ob?id=3")]
    [InlineData("/obs?page=1")]
    public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync(url);

        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        Assert.Equal("text/html; charset=utf-8", 
            response.Content.Headers.ContentType.ToString());
    }
}