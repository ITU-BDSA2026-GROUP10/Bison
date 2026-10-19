namespace Bison.Razor;

using System.Reflection.Metadata;
using DataModel;
using DTO;
using Microsoft.EntityFrameworkCore;

public interface IPostRepository
{
    public Task<List<PostDTO>> ReadPostWithID(int ID);
    public Task<List<PostDTO>> ReadPostsByAuthor(string author, int page);
    public Task<List<PostDTO>> ReadPosts(int page);
    public Task<List<PostDTO>> ReadPost(int PostID);
}
public class PostRepositoryClass : IPostRepository
{
    private readonly BisonDBContext _context;

    public PostRepositoryClass(BisonDBContext context)
    {
        _context = context;
        DbInitializer.SeedDatabase(_context);
    }

    public async Task<List<PostDTO>> ReadPostWithID(int ID)
    {
        var query = _context.Posts
            .Include(observation => observation.Author)
            .Where(observation => observation.PostId == ID);

        var result = await query.ToListAsync();
        var posts = new List<PostDTO>();
        foreach (var obs in result)
        {
            Console.WriteLine("------------------" + obs.Text + " " + obs.PostId + " " + obs.Author.Name);
            posts.Add(new PostDTO
            {
                Text = obs.Text,
                PostId = obs.PostId,
                AuthorName = obs.Author.Name,
                //make timestamp an int!
                TimeStamp = obs.TimeStamp.ToString() 
            });
        }

        

        return posts;
    }

    public async Task<List<PostDTO>> ReadPostsByAuthor(string author, int page)
    {
        int numberOfObs = 32;

        var query = _context.Posts
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

    public async Task<List<PostDTO>> ReadPosts(int page)
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

        public async Task<List<PostDTO>> ReadPost(int PostID)
        {
            return new List<PostDTO>();
        }
}