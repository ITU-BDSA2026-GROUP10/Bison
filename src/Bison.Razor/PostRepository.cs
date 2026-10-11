using System.Formats.Tar;

namespace Bison.Razor.PostRepository;
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
        Task<List<Post>> ReadPostsByAuthor(int AuthorID);
        Task<List<Post>> ReadPosts();
        Task<List<Post>> ReadPost(int PostID);
    }

    // Implementation
    public class PostRepository : IPostRepository
    {
        private readonly BisonDbContext _context;

        public PostRepository(BisonDbContext context)
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
            var query = dbContext.Posts
                        .Select(observation => new {observation.observation_id, observation.text, observation.author_id, observation.pub_date})
                        .Include(c => c.Author) //maybe this needs to be changed idk
                        .OrderByDescending(observation => observation.pub_date)
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
                    AuthorID = obs.AuthorID,
                    Timestamp = obs.Timestamp
                });
            }

            return posts;
        }

        private async Task<int> ReadPost(int PostID)
        {
            return new List<Post>();
        }
    }