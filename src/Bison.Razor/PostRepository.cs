namespace Bison.Razor.PostRepository;
using DataModel;
using Microsoft.EntityFrameworkCore;

//DTO

public class ObservationDTO
{ //TODO: DOUBLE CHECK THIS 
    public int PostID {get; set;}
    public int AuthorID {get; set;}
    public string Text {get; set;}
    public int Timestamp {get; set;}
}

// Interface
    public interface IPostRepository
    {
        public Task<List<Post>> ReadPostsByAuthor(int AuthorID);
        public Task<List<ObservationDTO>> ReadPosts(int page);
        public Task<List<Post>> ReadPost(int PostID);
    }

    // Implementation
    public class PostRepository : IPostRepository
    {
        private readonly BisonDBContext _context;

        public PostRepository(BisonDBContext context)
        {
            _context = context;
        }

        public async Task<List<Post>> ReadPostsByAuthor(int AuthorID)
        {
            return new List<Post>();
        }

        public async Task<List<ObservationDTO>> ReadPosts(int page)
        {
            int numberOfObs = 32;
            
            if (page == 0) 
            {
                page = 1;
            }
            // Define the query - with our setup, EF Core translates this to an SQLite query in the background
            var query = _context.Posts
                        .Select(observation => new {observation.PostID, observation.Text, observation.Author, observation.Timestamp})
                        //.Include(c => c.Author) //maybe this needs to be changed idk
                        .OrderByDescending(observation => observation.Timestamp)
                        .Skip(page * numberOfObs).Take(numberOfObs);
            // Execute the query and store the results
            var result = await query.ToListAsync();

            List<ObservationDTO> posts = new List<ObservationDTO>();
            foreach (var obs in result)
            {
                posts.Add(new ObservationDTO ()
                {
                    Text = obs.Text,
                    PostID = obs.PostID,
                    AuthorID = obs.Author.AuthorID,
                    //make timestamp an int!
                    //Timestamp = obs.Timestamp 
                });
            }

            return posts;
        }

        public async Task<List<Post>> ReadPost(int PostID)
        {
            return new List<Post>();
        }
    }