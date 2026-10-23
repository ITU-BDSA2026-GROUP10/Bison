namespace Bison.Razor;

using System.Reflection.Metadata;
using DataModel;
using DTO;
using Microsoft.EntityFrameworkCore;

public interface IPostRepository
{
    public Task<List<CommentDTO>> ReadCommentsWithID(int ID);
    public Task<List<PostDTO>> ReadPostsByAuthor(string author, int page);
    public Task<List<ObservationDTO>> ReadPosts(int page);
    public Task<List<ObservationDTO>> ReadPost(int PostID);
    public Task<List<ObservationDTO>> ReadPostWithID(int ID);
}
public class PostRepositoryClass : IPostRepository
{
    private readonly BisonDBContext _context;

    public PostRepositoryClass(BisonDBContext context)
    {
        _context = context;
    }

    public async Task<List<CommentDTO>> ReadCommentsWithID(int ID)
    {
    
        var commentQuery = _context.Comments
        .Include(comment => comment.Author)
        .Include(comment => comment.Text)
        .Where(comment => comment.PostId == ID);

        var result = await commentQuery.ToListAsync();

        List<CommentDTO> comments = new List<CommentDTO>();
        foreach (var comment in result)
        {
            comments.Add(new CommentDTO
            {
                ObservationID = comment.PostId,
                AuthorID = comment.AuthorId,
                AuthorName = comment.Author.Name,
                Text = comment.Text,
                TimeStamp = comment.TimeStamp.ToString()
            });
        }

        return comments;

    }

    public async Task<List<PostDTO>> ReadPostsByAuthor(string author, int page)
    {
        int numberOfObs = 32;

        var query = _context.Observations
        .Include(observation => observation.Author) //maybe this needs to be changed idk
        .Where(observation => observation.Author.Name == author)
        .OrderByDescending(observation => observation.TimeStamp)
        .Skip((page-1) * numberOfObs).Take(numberOfObs);

        var result = await query.ToListAsync();

        List<PostDTO> posts = new List<PostDTO>();
        foreach (var obs in result)
        {
            posts.Add(new PostDTO
            {
                Text = obs.Text,
                PostId = obs.PostId,
                AuthorName = obs.Author.Name,
                //AuthorID = obs.Author.AuthorID,
                //make timestamp an int!
                TimeStamp = obs.TimeStamp.ToString() 
            });
        }

        return posts;
    }

    public async Task<List<ObservationDTO>> ReadPosts(int page)
    {
        int numberOfObs = 32;
        
    
        // Define the query - with our setup, EF Core translates this to an SQLite query in the background
        var query = _context.Posts
        .Include(obs => obs.Author)
        .OrderByDescending(observation => observation.TimeStamp)
        .Skip((page-1) * numberOfObs).Take(numberOfObs);
                    // .Join(Authors, observation => observation.Author.AuthorID == Author.AuthorID)
                    // .Where(observation => observation.Author.AuthorID == User.user_id)
    
                    //.OrderByDescending(observation => observation.Timestamp);
                    /*.Select(observation => new {observation.PostID, observation.Text, observation.Author, observation.Timestamp})
                    .Skip(page * numberOfObs).Take(numberOfObs);*/
        // Execute the query and store the results
        var result = await query.ToListAsync();
        Console.WriteLine("HELLO!!!!!!!!!!!!!!!!!!!!!!!!!");
        Console.WriteLine("number of observations: " + result.Count);
        
        List<ObservationDTO> posts = new List<ObservationDTO>();
        foreach (var obs in result)
        {
            Console.WriteLine("observations tekst er: " + obs.Text);
            posts.Add(new ObservationDTO
            {
                PostID = obs.PostId,
                AuthorID = obs.Author.AuthorId,
                AuthorName = obs.Author.Name,
                Text = obs.Text,
                //make timestamp an int!
                TimeStamp = obs.TimeStamp.ToString()
            });
        }

        

        return posts;
    }

    public async Task<List<ObservationDTO>> ReadPostWithID(int ID)
    {
        var observationQuery = _context.Observations
            .Include(observation => observation.Author)
            .Where(observation => observation.PostId == ID);

        var observationResult = await observationQuery.ToListAsync();
        var posts = new List<ObservationDTO>();
        foreach (var obs in observationResult)
        {
            posts.Add(new ObservationDTO
            {
                PostID = obs.PostId,
                AuthorID = obs.Author.AuthorId,
                AuthorName = obs.Author.Name,
                Text = obs.Text,
                //make timestamp an int!
                TimeStamp = obs.TimeStamp.ToString()
            });
        }
        return posts;
    }

    public async Task<List<ObservationDTO>> ReadPost(int PostID)
    {
        var query = _context.Observations
        .Include(observation => observation.Author)
        .Include(observation => observation.Taxon)
        .Where(observation => observation.PostId == PostID);

        var observationResult = await query.ToListAsync();
        var posts = new List<ObservationDTO>();
        foreach (var obs in observationResult)
        {
            posts.Add(new ObservationDTO
            {
                PostID = obs.PostId,
                AuthorID = obs.Author.AuthorId,
                AuthorName = obs.Author.Name,
                Text = obs.Text,
                //make timestamp an int!
                TimeStamp = obs.TimeStamp.ToString()
            });
        }
        return posts;
    }
}