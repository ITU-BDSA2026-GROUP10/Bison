using Microsoft.AspNetCore.Mvc.Testing;
//using Microsoft.VisualStudio.TestPlatform.TestHost;
//using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Bison.Infrastructure.Services;
using Bison.Infrastructure.Repositories;
using System.Net;
using Bison.Core.Models;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Authorization;

namespace Bison.Tests;

/*test idea: 
    assert wether giving argument page nr 0 and 1 returns the same task list 
    in observationservice.getobservations(int) and GetObservationsFromAuthor(int)
    something similar can be done for the other methods if stm corrections
    the sut is observationservice.
    */

public class IntegrationTestsBasic : IClassFixture<WebApplicationFactory<Program>>
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

public class ObservationServiceIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    
    public ObservationServiceIntegrationTest (
        WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }
    
    //The SUT is ObservationService 
    //These integration tests are meant to take fake repos, databases and such but work on the real observationservice class
    [Fact]
    public async Task GetObservationsReturnsSameTaskWithPageNrOneAndZero()
    {
        //Arrange
        int one = 1;
        int zero = 0;
        IPostRepository repository = new SinglePostRepository();
        ObservationService service = new ObservationService(repository);
        
        //Act
        var result_One = await service.GetObservations(1);
        var result_Zero = await service.GetObservations(0);
        
        //Assert
        Assert.Equal(result_One, result_Zero);
    }
}

public class RazorPageIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program>
        _factory;

    public RazorPageIntegrationTests(
        CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }
    
    //See: https://github.com/dotnet/AspNetCore.Docs.Samples/blob/main/test/integration-tests/10.x/IntegrationTestsSample/tests/RazorPagesProject.Tests/IntegrationTests/IndexPageTests.cs
    
}

public class UserTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public UserTests (WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task UsersNotLoggedInCanAccessPublicTimeLine ()
    {
        // arrange
        // act
        var response = await _client.GetAsync("/obs");

        // assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    }

    [Fact]
    public async Task UsersNotLoggedInCannotPostObservations ()
    {
        // arrange
        Author author = new Author {Name = "Sofie", Email = "verysecret@mail.com"};
        var observation = new Post {Author = author, Text = "I saw a heron eating icecream", TimeStamp = DateTime.Now};

        // act
        var response = await _client.PostAsJsonAsync("/obs",observation);


        // assert
        Assert.NotEqual(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task UsersNotLoggedInHaveLogInButton ()
    {
        // act
        var response = await _client.GetAsync("/");

        // assert
        response.EnsureSuccessStatusCode();
        var element = await response.Content.ReadAsStringAsync();
        Assert.Contains("Register",element);
        Assert.Contains("Login",element);
    }

    [Fact]
    public async Task UsersNotLoggedInCannotLogOut ()
    {
        // act
        var response = await _client.GetAsync("/");

        // assert
        response.EnsureSuccessStatusCode();
        var element = await response.Content.ReadAsStringAsync();
        Assert.DoesNotContain("Logout",element);
    }
}

public class PostRepositoryIntegrationTests
{
    //fake context
    
}