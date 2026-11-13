using DataModel;
using NuGet.Protocol.Core.Types;

namespace Bison.Razor.ObservationService;
public record ObservationViewModel(string Author, string Message, string Timestamp);

public interface IObservationService
{
    public Task<List<ObservationViewModel>> GetObservations(int page);
    public Task<List<ObservationViewModel>> GetObservationsFromAuthor(string author, int page);
    public Task<List<ObservationViewModel>> GetObservationFromID(int id, int page);
    public Task<List<ObservationViewModel>> GetObservationsFromTaxonID(string id, int page);
    public void AddObservationPost(Author author, string text);
    public Author SetAuthor(string name, string email);
}

public class ObservationService : IObservationService
{
    //DBFacade dbf = new DBFacade();
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
            observations.Add(new ObservationViewModel(DTO.AuthorName, DTO.Text, DTO.TimeStamp));
        }

        return observations;
    } 

    public async Task<List<ObservationViewModel>> GetObservationsFromAuthor(string author, int page) {
        // filter by the provided author name

       page = ValidPageNumber(page);

        List<ObservationViewModel> observations = new List<ObservationViewModel>();
        var observationDTOs = await postRepository.ReadPostsByAuthor(author, page);

        foreach (var DTO in observationDTOs)
        {
            observations.Add(new ObservationViewModel(DTO.AuthorName, DTO.Text, DTO.TimeStamp));
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
            observations.Add(new ObservationViewModel(DTO.AuthorName, DTO.Text, DTO.TimeStamp.ToString()));
        }
        var commentDTOs = await postRepository.ReadCommentsWithID(id);
        
        foreach (var DTO in commentDTOs)
        {
            observations.Add(new ObservationViewModel(DTO.AuthorName, DTO.Text, DTO.TimeStamp.ToString()));
        }
        var proposalDTOs = await postRepository.ReadProposalsWithID(id);
        foreach (var DTO in proposalDTOs)
        {
            observations.Add(new ObservationViewModel(DTO.AuthorName, DTO.Text, DTO.TimeStamp.ToString()));
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
            observations.Add(new ObservationViewModel(DTO.AuthorName, DTO.Text, DTO.TimeStamp.ToString()));
        }
        
        return observations; 
    }
    
    public void AddObservationPost(Author author, string text)
    {
        int postid = postRepository.GetMaxPostId() + 1;
        
        Post obs = new Post(author, text, postid);
        postRepository.AddPost(obs);
    }
    
    public Author SetAuthor(string name, string email)
    {
        Author author = new Author(name, email);
        int result = Convert.ToInt32(postRepository.GetAuthorIdFromAuthor(author));
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

    public static int ValidPageNumber(int page) {
        if (page <= 0)
        {
            page = 1;
        }

        return page;
    }

}
