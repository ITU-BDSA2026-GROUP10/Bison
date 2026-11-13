namespace Bison.Razor.tests;
using System;
using System.Reflection;
using Bison.Razor.ObservationService;
using Bison.Razor;
using Microsoft.EntityFrameworkCore;
using DataModel;
using DTO;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

public class UnitTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void ReturnsPageOne(int page)
    {

        //Act
        int result = ObservationService.ValidPageNumber(page);

        //Assert
        Assert.Equal(1, result);
    }

    
    [Fact]
    public void ValidPageNumberReturnsSamePage() 
    {
        //Arrange 
        int page = 3;

        //Act
        int result = ObservationService.ValidPageNumber(page);

        //Assert
        Assert.Equal(3, result);
    }

    [Fact]
    public async Task EmptyObservationsReturnsEmptyList()
    {
        //Arrange 
        EmptyPostRepository repository = new EmptyPostRepository();
        ObservationService service = new ObservationService(repository);

        //Act
        var result = await service.GetObservations(1);


        //Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task EmptyAuthorObservationsReturnsEmptyList()
    {
        //Arrange 
        EmptyPostRepository repository = new EmptyPostRepository();
        ObservationService service = new ObservationService(repository);
        String author = "Raamin";

        //Act
        var result = await service.GetObservationsFromAuthor(author, 1);


        //Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task AuthorObservationsReturnsCorrectData()
    {
        //Arrange 
        SinglePostRepository repository = new SinglePostRepository();
        ObservationService service = new ObservationService(repository);
        String author = "Raamin";

        //Act
        var result = await service.GetObservationsFromAuthor(author, 1);
        var observation = result[0];

        //Assert
        Assert.NotEmpty(result);
        Assert.Equal("Raamin", observation.Author);
        Assert.Equal("I saw a penguin", observation.Message);
        Assert.Equal("10/25/26 12:00:00", observation.Timestamp);
    }


    [Fact]
    public async Task GetObservationReturnsCorrectData()
    {
        //Arrange 
        SinglePostRepository repository = new SinglePostRepository();
        ObservationService service = new ObservationService(repository);
    

        //Act
        var result = await service.GetObservations(1);
        var observation = result[0];


        //Assert
        Assert.Equal(1, result.Count);
        Assert.Equal("Raamin", observation.Author);
        Assert.Equal("I saw a penguin", observation.Message);
        Assert.Equal("10/25/26 12:00:00", observation.Timestamp);
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

public class EmptyPostRepository : IPostRepository
{
    public Task<List<ObservationDTO>> ReadPosts(int page) 
    {
        //https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task.fromresult?view=net-10.0
        //This line returns an already completed task containing an empty list 
        return Task.FromResult(new List<ObservationDTO>());
    }

    //The methods cannot be left empty becuase error CS0161: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/compiler-messages/jump-statement-errors
    //Instead use this exception: https://learn.microsoft.com/en-us/dotnet/api/system.notimplementedexception?view=net-10.0
    public Task<List<CommentDTO>> ReadCommentsWithID(int ID)
    {
        throw new NotImplementedException();
    }
    public Task<List<ObservationDTO>> ReadPostsByAuthor(string author, int page)
    {
        return Task.FromResult(new List<ObservationDTO>());
    }
    public Task<List<ObservationDTO>> ReadPostWithID(int ID)
    {
        throw new NotImplementedException();
    }
    public Task<List<ProposalDTO>> ReadProposalsWithID(int ID)
    {
        throw new NotImplementedException();
    }
    public async Task <List<ObservationDTO>> ReadObservationsWithTaxonId(string id, int page/*, List<Observation> observations*/) 
    {
         throw new NotImplementedException();
    }
     public async Task<List<ObservationDTO>> ReadPosts()
    {
        throw new NotImplementedException();
    }

    public int GetMaxPostId() { return 0;}
    public void AddPost (Post post) {}
    public Task<Boolean> DoesThisAuthorExist (Author author) { return null;}
    public Task<int> GetAuthorIdFromAuthor (Author author) { return null;}
    public int GetMaxAuthorId() { return 0;}
}

public class SinglePostRepository : IPostRepository
{
    public Task<List<ObservationDTO>> ReadPosts(int page) 
    {
        ObservationDTO observation = new ObservationDTO 
        {
            AuthorName = "Raamin",
            Text = "I saw a penguin",
            TimeStamp = "10/25/26 12:00:00",
        };
        return Task.FromResult(new List<ObservationDTO>{observation});
    }

    //The methods cannot be left empty becuase error CS0161: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/compiler-messages/jump-statement-errors
    //Instead use this exception: https://learn.microsoft.com/en-us/dotnet/api/system.notimplementedexception?view=net-10.0
    public Task<List<CommentDTO>> ReadCommentsWithID(int ID)
    {
        throw new NotImplementedException();
    }
    public Task<List<ObservationDTO>> ReadPostsByAuthor(string author, int page)
    {
         ObservationDTO observation = new ObservationDTO 
        {
            AuthorName = author,
            Text = "I saw a penguin",
            TimeStamp = "10/25/26 12:00:00",
        }; 
        return Task.FromResult(new List<ObservationDTO>{observation});
    }
    public Task<List<ObservationDTO>> ReadPostWithID(int ID)
    {
        throw new NotImplementedException();
    }
    public Task<List<ProposalDTO>> ReadProposalsWithID(int ID)
    {
        throw new NotImplementedException();
    }
    public async Task<List<ObservationDTO>> ReadPosts()
    {
        throw new NotImplementedException();
    }
    public async Task <List<ObservationDTO>> ReadObservationsWithTaxonId(string id, int page/*, List<Observation> observations*/) 
    {
         throw new NotImplementedException();
    }

    public int GetMaxPostId() { return 0;}
    public void AddPost (Post post) {}
    public Task<Boolean> DoesThisAuthorExist (Author author) { return null;}
    public Task<int> GetAuthorIdFromAuthor (Author author) { return null;}
    public int GetMaxAuthorId() { return 0;}
}


// https://moleseyhill.com/2009-07-06-unit-testing-with-repository-pattern.html
public class FakePostRepository : IPostRepository
{
    private readonly BisonDBContext _context;
    // new BisonDBContext(Configuration.ConnectionStrings["FakeRepoTest"].ConnectionString);

    public FakePostRepository()
    { }

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
    public async Task<List<ObservationDTO>> ReadPosts()
    {
        throw new NotImplementedException();
    }
    public async Task <List<ObservationDTO>> ReadObservationsWithTaxonId(string id, int page/*, List<Observation> observations*/) 
    {
         throw new NotImplementedException();
    }
    //The following are simply not written yet:
    public async Task<List<ProposalDTO>> ReadProposalsWithID(int p){ return null;}
    
    public async Task<List<ObservationDTO>> ReadPostWithID(int p) {return null;}
    
    public async Task<List<ObservationDTO>> ReadPostsByAuthor(string author, int p){return null;}
    
    public async Task<List<CommentDTO>> ReadCommentsWithID(int p){return null;}

    public int GetMaxPostId() { return 0;}
    public void AddPost (Post post) {}
    public Task<Boolean> DoesThisAuthorExist (Author author) { return null;}
    public Task<int> GetAuthorIdFromAuthor (Author author) { return null;}
    public int GetMaxAuthorId() { return 0;}
}