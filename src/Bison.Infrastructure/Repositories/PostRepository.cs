using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;
using Bison.Core.Models;
using Bison.Core.DTOs;
using Bison.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Bison.Infrastructure.Repositories;

public interface IPostRepository
{
    public Task<List<CommentDTO>> ReadCommentsWithID(int ID);
    public Task<List<ObservationDTO>> ReadPostsByAuthor(string author, int page);
    public Task<List<ObservationDTO>> ReadPosts(int page);
    public Task<List<ObservationDTO>> ReadPostWithID(int ID);
    public Observation ReadObservationWithID(int ID);
    public Task<List<ProposalDTO>> ReadProposalsWithID(int ID);
    public Task<List<ObservationDTO>> ReadObservationsWithTaxonId(string id, int page);
    public Task<List<ObservationDTO>> ReadPosts();
    public Taxon ReadTaxonWithID(int id);
    public int GetMaxPostId();
    public Task<int> AddPost(Observation observation);
    public Task<Author> GetAuthorFromDatabase(string name, string email);
    public Task<Boolean> DoesThisAuthorExist(Author author);
    public Task<int> GetAuthorIdFromAuthor(Author author);
    public int GetMaxAuthorId();
    public Task<int> AddComments(Comment comment);
    public Task<int> AddProposals(Proposal proposal);

    public Task <int> AddFavorites(Author author, int id);

    public Task<int> AddAuthor(Author author, int authorId);
    //public Task<int> GetFavoritesFromAuthor(Author author);
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

    public Taxon ReadTaxonWithID(int id)
    {
        var taxonQuery = _context.Taxons
           .Where(taxon => taxon.TaxonId == id);

        var result = taxonQuery.ToList();
        var tax = result.FirstOrDefault();
        Console.WriteLine(" !!!! this is the taxon we think we found ------------- " + tax);

        return tax;
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

    /*private Observation GetObservationsFromId(Observation obs)
    {
        var posts = new Observation
            {
                PostId = obs.PostId,
                AuthorId = obs.Author.AuthorId,
                Author = obs.Author,
                Text = obs.Text,
                TimeStamp = obs.TimeStamp,
                Taxon = obs.Taxon
            };
        
        return posts;
    }*/
    
    public Observation ReadObservationWithID(int ID)
    {
        var observationQuery = _context.Observations
            .Include(observation => observation.Author)
            .Where(observation => observation.PostId == ID);

        var result = observationQuery.ToList();
        var obs = result.FirstOrDefault();
        Console.WriteLine(" !!!! this is the obs we think we found ------------- "+ obs);
        
        return obs;
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

    public async Task<int> AddPost (Observation obs)
    {
        var auth = obs.Author;
        if (await DoesThisAuthorExist(auth))
        {
            auth = await GetAuthorFromDatabase(auth.Name, auth.Email);
        }
        await _context.Observations.AddAsync(obs);
       //_context.Add(post);
        await _context.SaveChangesAsync();
        return obs.PostId;
    } 

    public async Task<Author> GetAuthorFromDatabase(string name, string email)
{
    var author = await _context.Authors
        .FirstOrDefaultAsync(a => a.Name == name && a.Email == email);

    if (author != null)
    {
        return author;
    }

    author = new Author{Name = name, Email = email};

    _context.Authors.Add(author);
    await _context.SaveChangesAsync();

    return author;
}
    
    public int GetMaxPostId()
    {
        int max = _context.Posts.Max(row => row.PostId);
        
        return max;
    }

    public async Task<Boolean> DoesThisAuthorExist (Author author)
    {
        var query = _context.Observations.Where(observation => observation.Author.Name == author.Name);

        var result = await query.ToListAsync();
        
        if (result.Count == 0)
        {
            return false;
        } else {
            return true;
        }
    }
    public async Task<int> GetAuthorIdFromAuthor(Author author)
    {
        var authorExists = await DoesThisAuthorExist(author);
        if (authorExists)
        {
            var query = _context.Authors
            .Where(auth => auth.AuthorId == author.AuthorId);
            
            var result = await query.ToListAsync();
            var auth = result.FirstOrDefault();
            Console.WriteLine("Author exists");
            return author.AuthorId;
        } else
        {
            Console.WriteLine("Author doesn't exist");
            return GetMaxAuthorId() + 1;
        }
    }

    public int GetMaxAuthorId()
    {
        int max = _context.Observations.Max(row => row.AuthorId);
        
        return max;
    }
    
    public async Task<int> AddAuthor(Author author, int authorId)
    {
        if (author.AuthorId == null && authorId != null){author.AuthorId = authorId;}
        _context.Authors.Add(author);
        await _context.SaveChangesAsync();
        return author.AuthorId;
    }
    
    //--- comments and proposals -----
     public async Task<int> AddComments(Comment comment)
    {
        comment.Author.Posts?.Add(comment);
        comment.Observation.Comments?.Add(comment);
        await _context.Posts.AddAsync(comment);
        await _context.Comments.AddAsync(comment);
        // _context.Add(comment);
        await _context.SaveChangesAsync();
        return comment.PostId;
    } 
    
     public async Task<int> AddProposals(Proposal proposal)
    {
        proposal.Author.Posts.Add(proposal);
        proposal.Observation.Proposals.Add(proposal);
       _context.Posts.Add(proposal);
       _context.Proposals.Add(proposal);
        // _context.Add(comment);
        await _context.SaveChangesAsync();
        return proposal.PostId;
    } 
    
    public async Task<int> AddFavorites(Author author, int id)
    {
        author.FavoritePosts.Add(id);
        return id;
    }

    /*public async Task<List<ObservationDTO>> GetFavoritesFromAuthor(Author author)
    {
        List<ObservationDTO> observationDTOs = new List<ObservationDTO>();
        var observationDTOs = await ReadPostWithID(id);
        return author.FavoritePosts;
    }*/
}