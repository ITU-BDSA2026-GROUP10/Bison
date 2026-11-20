using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Transactions;
using Bison.Core.Models;
using Bison.Infrastructure.Repositories;

namespace Bison.Infrastructure.Services;
public record ObservationViewModel(string Author, string Message, string Timestamp, int? PostId, Type? type);


public interface IObservationService
{
    public Task<List<ObservationViewModel>> GetObservations(int page);

    public Task<List<ObservationViewModel>> ReadCommentsWithID(int obID, int page);
    public Task<List<ObservationViewModel>> GetObservationsFromAuthor(string author, int page);
    public Task<List<ObservationViewModel>> GetObservationFromID(int id, int page);
    public Task<List<ObservationViewModel>> GetObservationsFromTaxonID(string id, int page);
    public void AddObservationPost(Author author, string text);

    public Task AddCommentPost(Author author, string text, int obID);

    public Task AddProposalPost(Author author, string text, int obId, int taxonId);
    public Task<Author> SetAuthor(string name, string email);
}

public class ObservationService : IObservationService
{
    
    private IPostRepository postRepository;
    private /*readonly*/List<ObservationViewModel> _obs;

    public ObservationService(IPostRepository postRepository)
    {
        this.postRepository = postRepository;
    }

    public async Task<List<ObservationViewModel>> GetObservations(int page)
    {
        page = ValidPageNumber(page);

        List<ObservationViewModel> observations = new List<ObservationViewModel>();
        var observationDTOs = await postRepository.ReadPosts(page);

        foreach (var DTO in observationDTOs)
        {
            observations.Add(new ObservationViewModel(DTO.AuthorName, DTO.Text, DTO.TimeStamp, DTO.PostID, typeof(Observation)));
        }

        return observations;
    } 

    public async Task<List<ObservationViewModel>> ReadCommentsWithID(int obID, int page)
    {
        page = ValidPageNumber(page);
        
        List<ObservationViewModel> comments = new List<ObservationViewModel>();
        var commentDTOs = await postRepository.ReadCommentsWithID(obID);
        
       foreach (var DTO in commentDTOs)
        {
            //comments.Add(new ObservationViewModel(DTO.AuthorName, DTO.Text, DTO.TimeStamp));
        }
        
        return comments;
    }

    public async Task<List<ObservationViewModel>> GetObservationsFromAuthor(string author, int page) 
    {
        // filter by the provided author name

       page = ValidPageNumber(page);

        List<ObservationViewModel> observations = new List<ObservationViewModel>();
        var observationDTOs = await postRepository.ReadPostsByAuthor(author, page);

        foreach (var DTO in observationDTOs)
        {
            observations.Add(new ObservationViewModel(DTO.AuthorName, DTO.Text, DTO.TimeStamp, DTO.PostID, typeof(Observation)));
        }

        return observations;
    }

    public async Task<List<ObservationViewModel>> GetObservationFromID(int id, int page)
    {

        if (id == 0) 
        {
            return await GetObservations(1); //has to include await since we're returning a task
        } else if (page == 0)
        {
            page = 1;
        }

        List<ObservationViewModel> observations = new List<ObservationViewModel>();
        var observationDTOs = await postRepository.ReadPostWithID(id);
        foreach (var DTO in observationDTOs)
        {
            observations.Add(new ObservationViewModel(DTO.AuthorName, DTO.Text, DTO.TimeStamp.ToString(), DTO.PostID, typeof(Observation)));
        }
        var commentDTOs = await postRepository.ReadCommentsWithID(id);
        
        foreach (var DTO in commentDTOs)
        {
            observations.Add(new ObservationViewModel(DTO.AuthorName, DTO.Text, DTO.TimeStamp.ToString(), DTO.ObservationID, typeof(Comment)));
        }
        var proposalDTOs = await postRepository.ReadProposalsWithID(id);
        foreach (var DTO in proposalDTOs)
        {
            observations.Add(new ObservationViewModel(DTO.AuthorName, DTO.Text, DTO.TimeStamp.ToString(), DTO.ObservationID, typeof(Proposal)));
        }
        return observations; 
    }

    public async Task<List<ObservationViewModel>> GetObservationsFromTaxonID(string id, int page)
    {

        page = ValidPageNumber(page);

        List<ObservationViewModel> observations = new List<ObservationViewModel>();
        var observationDTOs = await postRepository.ReadObservationsWithTaxonId(id, page);
        foreach (var DTO in observationDTOs)
        {
            observations.Add(new ObservationViewModel(DTO.AuthorName, DTO.Text, DTO.TimeStamp.ToString(), DTO.PostID, typeof(Observation)));
        }
        
        return observations; 
    }
    
    public void AddObservationPost(Author author, string text)
    {
        int postid = postRepository.GetMaxPostId() + 1;

        Observation obs = new Observation
        {
            Author = author,
            Text = text,
            PostId = postid,
            TimeStamp = DateTime.Now
        };
        
        Console.WriteLine(obs.Author.Email);
        Console.WriteLine(text);
        postRepository.AddPost(obs);
    }
    
    public async Task AddCommentPost(Author author, string text, int obId)
    {
        int postid = postRepository.GetMaxPostId() + 1;
        Observation observation = postRepository.ReadObservationWithID(obId);
        Comment c = new Comment
        {
            Author = author,
            Text = text,
            PostId = postid,
            Observation = observation,
            TimeStamp = DateTime.Now
        };
        
        await postRepository.AddComments(c);
    }
    
    public async Task AddProposalPost (Author author, string text, int obId, int taxonId)
    {
        int postid = postRepository.GetMaxPostId() + 1;
        Observation observation = postRepository.ReadObservationWithID(obId);
        Taxon tax = postRepository.ReadTaxonWithID(taxonId);
        
        Proposal p = new Proposal
        {
            Author = author,
            Observation = observation,
            Text = text,
            Taxon = tax,
            PostId = postid,
            TimeStamp = DateTime.Now
        };
        
        await postRepository.AddProposals(p);
    }
    
    public async Task<Author> SetAuthor(string name, string email)
    {
        Author author = new Author{Name = name, Email = email};
        var result = 0;
        if (await postRepository.DoesThisAuthorExist(author))
        {
            result = await postRepository.GetAuthorIdFromAuthor(author);
        } else
        {
            result = postRepository.GetMaxAuthorId()+1;
            await postRepository.AddAuthor(author, result);
        }
        author.AuthorId = result;
        return author;
    }

    public static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("G", System.Globalization.DateTimeFormatInfo.InvariantInfo);
    }

    public static int ValidPageNumber(int page) 
    {
        if (page <= 0)
        {
            page = 1;
        }

        return page;
    }

}
