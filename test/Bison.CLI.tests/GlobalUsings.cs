global using Xunit;

using SimpleDB;
using System.Net;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http.HttpResults;

public class GlobalUsings
{
    [Fact]
    public async Task PostObservations_Returns200()
    {
        // Arrange
        var client = new HttpClient();
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        client.BaseAddress = new Uri("http://localhost:5252");
        string observation = "Heron at Ismageriet";
        string location = "Ismageriet";
        // Act
        var response = await client.PostAsJsonAsync($"http://localhost:5252/observation?observation={observation}&location={location}", new {observation, location});
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        /*var observations = await response.Content.ReadFromJsonAsync<List<Observations>>();
        Assert.NotNull(observations);
        Assert.NotEmpty(observations); */
    }       

    [Fact]
    public async Task GetObservations_Returns200_AndObservations()
    {
        //Arrange
        var client = new HttpClient();
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        client.BaseAddress = new Uri("http://localhost:5252");
        // Act
        var observation = await client.GetFromJsonAsync<IEnumerable<Observations>>("http://localhost:5252/observations");
        // Assert
        HttpResponseMessage response = await client.GetAsync("/observations");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var observations = await response.Content.ReadFromJsonAsync<List<Observations>>();
        Assert.NotNull(observations);
        Assert.NotEmpty(observations); 
    }
}