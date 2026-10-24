namespace Bison.Razor.tests;
using System;
using System.Reflection;
using Bison.Razor.BisonService;
using Bison.Razor;
using Microsoft.EntityFrameworkCore;
using DataModel;
using DTO;
using Microsoft.Data.Sqlite;

public class UnitTests
{
    [Fact]
    public void UnixTimeConvertsCorrectlyToUserReadableTime()
    {
        //Arrange
        long unixTime = 1789313646+7200;
        string am = "09/13/26 17.34.06";
        string dk = "09-13-26 17:34:06";
        string uk = "09.13.26 17.34.06";
        
        //Act
        string dateTime = ObservationService.UnixTimeStampToDateTimeString(unixTime);

        //Assert
        Assert.True(am == dateTime || dk == dateTime || uk == dateTime);
    }
    
    /*
    [Fact]
    public void CorrectPageIsReturned()
    {
        //Arrange
        int page = 1;
        
        //Act
        var taskList = PostRepositoryClass.ReadPosts(page);

        //Assert
        Assert.NotNull(taskList);
    }
    */

    [Fact]
    public void PaginationPublicOnGetReturnsValidPageNumber()
    {
        //Arrange
        int nr = 4;
        /*
        ObservationService observationsservice = new ObservationService(new FakePostRepository(new BisonDBContext()));

        //Act
        var task = observationsservice.GetObservations(nr);

        MethodInfo? method = typeof(IObservationService).GetMethod("GetObservation");
        //Assert
        */

    }


    [Fact]
    public async Task MessageRepositoryUnitTest1()
    {
        // Arrange
        /*using var connection = new SqliteConnection("Filename=:memory:");
        await connection.OpenAsync();
        BisonDBContext _context;
        var builder = _context.UseSqlite(connection);
        using var context = new BisonDBcontext(builder.Options);
        await context.Database.EnsureCreatedAsync(); // Applies the schema to the database
        IPostRepository repository = new FakePostRepository(context);
        IObservationService service = new ObservationService(repository);
        int nr = 4;*/
        
        
        // Act
        //var result = repository.ReadPosts(nr);
        //var task = service.GetObservations(nr);

        //Assert
        //Assert.NotNull(result);
        

    }
}

// https://moleseyhill.com/2009-07-06-unit-testing-with-repository-pattern.html
public class FakePostRepository : IPostRepository
{
    private readonly BisonDBContext _context;

    public FakePostRepository(BisonDBContext context)
    {
        _context = context;
    }

    public async Task<List<ObservationDTO>> ReadPosts(int page)
    {
        int numberOfObs = 32;
        
        // Define the query - with our setup, EF Core translates this to an SQLite query in the background
        var query = _context.Observations
            .Include(obs => obs.Author)
            .OrderByDescending(observation => observation.TimeStamp)
            .Skip((page-1) * numberOfObs).Take(numberOfObs);
        
        // Execute the query and store the results
        var result = await query.ToListAsync();
       
        return GetObservationDTO(result);
    }
    private List<ObservationDTO> GetObservationDTO(List<Observation> queryResult)
    {
        var posts = new List<ObservationDTO>();
        foreach (var obs in queryResult)
        {
            posts.Add(new ObservationDTO
            {
                PostID = obs.PostId,
                AuthorID = obs.Author.AuthorId,
                AuthorName = obs.Author.Name,
                Text = obs.Text,
                TimeStamp = obs.TimeStamp.ToString()
            });
        }
        return posts;
    }
    
    //The following are simply not written yet:
    public async Task<List<ProposalDTO>> ReadProposalsWithID(int p){ return null;}
    
    public async Task<List<ObservationDTO>> ReadPostWithID(int p) {return null;}
    
    public async Task<List<ObservationDTO>> ReadPostsByAuthor(string author, int p){return null;}
    
    public async Task<List<CommentDTO>> ReadCommentsWithID(int p){return null;}
}