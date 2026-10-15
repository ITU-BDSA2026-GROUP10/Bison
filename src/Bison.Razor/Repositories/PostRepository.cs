namespace Bison.Razor.PostRepository;

using System.Reflection.Metadata;
using DataModel;
using Microsoft.EntityFrameworkCore;
    
public class PostRepositoryClass : IPostRepository
{
    private readonly BisonDBContext _context;

    public PostRepositoryClass(BisonDBContext context)
    {
        _context = context;
    }

    public async Task<List<PostDTO>> ReadPostsByAuthor(int AuthorID)
    {
        return new List<PostDTO>();
    }

    public async Task<List<PostDTO>> ReadPosts(int AuthorID)
    {
            /*int numberOfObs = 32;
            
            if (page == 0) 
            {
                page = 1;
            }*/
            // Define the query - with our setup, EF Core translates this to an SQLite query in the background
            var query = _context.Posts
                        // .Join(Authors, observation => observation.Author.AuthorID == Author.AuthorID)
                        // .Where(observation => observation.Author.AuthorID == User.user_id)
                        .Include(observation => observation.Author) //maybe this needs to be changed idk
                        .Where(observation => observation.Author == AuthorID);
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
                    PostID = obs.PostID,
                    AuthorName = obs.Author.Name
                    //AuthorID = obs.Author.AuthorID,
                    //make timestamp an int!
                    //Timestamp = obs.Timestamp 
                });
            }

            return posts;
        }

        public async Task<List<PostDTO>> ReadPost(int PostID)
        {
            return new List<PostDTO>();
        }
    }