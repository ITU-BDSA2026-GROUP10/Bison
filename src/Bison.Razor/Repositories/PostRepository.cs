namespace Bison.Razor;

using System.Reflection.Metadata;
using DataModel;
using DTO;
using Microsoft.EntityFrameworkCore;

public interface IPostRepository
{
    public Task<List<CommentDTO>> ReadCommentsWithID(int ID);
    public Task<List<ObservationDTO>> ReadPostsByAuthor(string author, int page);
    public Task<List<ObservationDTO>> ReadPosts(int page);
    public Task<List<ObservationDTO>> ReadPostWithID(int ID);
    public Task<List<ProposalDTO>> ReadProposalsWithID(int ID);
    public Task <List<ObservationDTO>> ReadObservationsWithTaxonId(string id, int page);
    public Task<List<ObservationDTO>> ReadPosts();

    public int GetMaxPostId();

    public void AddPost (Post post);
}
public class PostRepositoryClass : IPostRepository
{
    private readonly BisonDBContext _context;

    public PostRepositoryClass(BisonDBContext context)
    {
        _context = context;
    }

    public async Task<List<ObservationDTO>> ReadPosts(int page)
    {
        int numberOfObs = 32;
        
        // Define the query - with our setup, EF Core translates this to an SQLite query in the background
        var query = _context.Observations
            .Include(obs => obs.Author)
            .Include(obs => obs.Taxon)
            .OrderByDescending(observation => observation.TimeStamp)
            .Skip((page-1) * numberOfObs).Take(numberOfObs);
        
        // Execute the query and store the results
        var result = await query.ToListAsync();
       
        return GetObservationDTO(result);
    }

    public async Task<List<ObservationDTO>> ReadPostsByAuthor(string author, int page)
    {
        int numberOfObs = 32;

        var query = _context.Observations
            .Include(observation => observation.Author) //maybe this needs to be changed idk
            .Where(observation => observation.Author.Name == author)
            .OrderByDescending(observation => observation.TimeStamp)
            .Skip((page-1) * numberOfObs).Take(numberOfObs);

        var result = await query.ToListAsync();

        return GetObservationDTO(result);
    }

    public async Task<List<ObservationDTO>> ReadPostWithID(int ID)
    {
        var observationQuery = _context.Observations
            .Include(observation => observation.Author)
            .Where(observation => observation.PostId == ID);

        var result = await observationQuery.ToListAsync();
        
        return GetObservationDTO(result);
    }

    public async Task<List<CommentDTO>> ReadCommentsWithID(int ID)
    {
    
        var commentQuery = _context.Comments
        .Include(comment => comment.Author)
        .Where(comment => comment.Observation.PostId == ID);

        var result = await commentQuery.ToListAsync();

        return GetCommentDTOs(result);
    }

    public async Task<List<ProposalDTO>> ReadProposalsWithID(int ID)
    {
        var proposalQuery = _context.Proposals
        .Include(proposal => proposal.Author)
        .Where(proposal => proposal.Observation.PostId == ID);
        var result = await proposalQuery.ToListAsync();

        return GetProposalDTOs(result);
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
                //make timestamp an int!
                TimeStamp = obs.TimeStamp.ToString(),
                Taxon = obs.Taxon
            });
        }
        return posts;
    }
    
    private List<CommentDTO> GetCommentDTOs(List<Comment> queryResult)
    {
        List<CommentDTO> comments = new List<CommentDTO>();
        
        foreach (var comment in queryResult)
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

    private List<ProposalDTO> GetProposalDTOs(List<Proposal> queryResult)
    {
        var proposals = new List<ProposalDTO>();
        
        foreach (var proposal in queryResult)
        {
            proposals.Add(new ProposalDTO
            {
                AuthorName = proposal.Author.Name,
                Text = proposal.Text,
                TimeStamp = proposal.TimeStamp.ToString()
            });
        }

        return proposals;
    }

    public async Task <List<ObservationDTO>> ReadObservationsWithTaxonId(string id, int page/*, List<Observation> observations*/) 
    {
        //var observationsList = new List <ObservationDTO>();
        
        var observationQuery = _context.Observations
        .Include(observation => observation.Author)
        .Where(observation => observation.Taxon.dwc_TaxonID == id);
        var result = await observationQuery.ToListAsync();

        return GetObservationDTO(result);
    }
    
    public async Task<List<ObservationDTO>> ReadPosts()
    {
        var query = _context.Observations
            .Include(obs => obs.Author)
            .Include(obs => obs.Taxon)
            .OrderByDescending(observation => observation.TimeStamp);
        
        // Execute the query and store the results
        var result = await query.ToListAsync();
       
        return GetObservationDTO(result);
    }

     public void AddPost (Post post)
    {
        var observationQuery = _context.Posts.Add(post);
    
    }
    
    public int GetMaxPostId()
    {
        int max = _context.Observations.Max(row => row.PostId);
        
        return max;
    }
}