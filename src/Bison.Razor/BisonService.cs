namespace Bison.Razor.BisonService;
using Bison.Razor.PostRepository;

public record ObservationViewModel(string Author, string Message, string Timestamp);

public interface IObservationService
{
    public Task<List<ObservationViewModel>> GetObservations(int page);
    public Task<List<ObservationViewModel>> GetObservationsFromAuthor(int AuthorID, int page);
    
    public Task<List<ObservationViewModel>> GetObservationFromID(int id, int page);
}

public class ObservationService : IObservationService
{
    DBFacade dbf = new DBFacade();
    private PostRepositoryClass postRepository;
    
    // These would normally be loaded from a database for example
    private /*readonly*/List<ObservationViewModel> _obs;

    public ObservationService(PostRepositoryClass postRepository)
    {
        this.postRepository = postRepository;
    }


    public async Task<List<ObservationViewModel>> GetObservations(int page)
    {
        int numberOfObs = 32;

        if (page == 0) 
        {
            page = 1;
        }

        List<ObservationViewModel> observations = new List<ObservationViewModel>();
        var observationDTOs = await postRepository.ReadPosts();

        foreach (var DTO in observationDTOs)
        {
            observations.Add(new ObservationViewModel(DTO.AuthorName, DTO.Text, DTO.Timestamp));
        }

        return observations;

        /*_obs = 
            dbf.getObservationsFromDatabase("SELECT observation.*, user.* FROM observation, user WHERE observation.author_id = user.user_id"
            + $" ORDER BY observation.pub_date DESC LIMIT " + numberOfObs + " OFFSET " + numberOfObs * (page-1) +";");
        return _obs;*/
        // return observations;
    } 

    public async Task<List<ObservationViewModel>> GetObservationsFromAuthor(int AuthorID, int page) {
        // filter by the provided author name

        List<ObservationViewModel> observations = new List<ObservationViewModel>();
        var observationDTOs = await postRepository.ReadPostsByAuthor(AuthorID);

        foreach (var DTO in observationDTOs)
        {
            observations.Add(new ObservationViewModel(DTO.AuthorName, DTO.Text, DTO.Timestamp));
        }


        /*_obs = 
            dbf.getObservationsFromDatabase("SELECT observation.*, user.* FROM observation, user WHERE observation.author_id = user.user_id" + 
            " AND user.username = @author" + 
            $" ORDER BY observation.pub_date DESC LIMIT " + numberOfObs + " OFFSET " + numberOfObs * (page-1) +";", author);
        return _obs;*/

        return observations;

        //return _obs.Where(x => x.Author == author).ToList();
    }

    public async Task<List<ObservationViewModel>> GetObservationFromID(int id, int page)
    {
        int numberOfObs = 32;

        if (id == 0) 
        {
            return await GetObservations(1); //has to include await since we're returning a task
        } else if (page == 0)
        {
            page = 1;
        }

       // _obs = 
           // dbf.getObservationsFromDatabase("SELECT observation.*, user.* FROM observation, user WHERE observation.author_id = user.user_id and observation_id = " + id
           // + $" ORDER BY observation.pub_date DESC LIMIT " + numberOfObs + " OFFSET " + numberOfObs * (page-1) +";");
            
        string[] queries = new string[3];
        queries[0] = "SELECT observation.*, user.* FROM observation, user WHERE observation.author_id = user.user_id and observation.observation_id = " + id
            + $" ORDER BY observation.pub_date DESC LIMIT " + numberOfObs + " OFFSET " + numberOfObs * (page-1) +";"; 
        
        queries[1] = "SELECT observation.*, user.*, comment.* FROM observation, user, comment WHERE comment.author_id = user.user_id and observation.observation_id = " + id + " and observation.observation_id = comment.observation_id"
            + $" ORDER BY observation.pub_date DESC LIMIT " + numberOfObs + " OFFSET " + numberOfObs * (page-1) +";";

        queries[2] = "SELECT observation.*, user.*, proposal.* FROM observation, user, proposal WHERE proposal.author_id = user.user_id and observation.observation_id = " + id + " and observation.observation_id = proposal.observation_id"
            + $" ORDER BY observation.pub_date DESC LIMIT " + numberOfObs + " OFFSET " + numberOfObs * (page-1) +";"; 

        /*  queries[1] = "SELECT comment.observation_id, user.* FROM comment, user WHERE comment_author_id_c = user.user_id and comment.observation_id = " + id 
        + $" ORDER BY observation.pub_date DESC LIMIT " + numberOfObs + " OFFSET " + numberOfObs * (page-1) +";";
        queries[2] = "SELECT proposal.observation_id, user.* FROM proposal, user WHERE proposal_author_id_p = user.user_id and proposal.observation_id = " + id 
        + $" ORDER BY observation.pub_date DESC LIMIT " + numberOfObs + " OFFSET " + numberOfObs * (page-1) +";"; */

        /*string query = "SELECT observation.*, user.* FROM observation, user INNER JOIN comment on observation.observation_id = comment.observation_id INNER JOIN proposal on observation.observation_id = proposal.observation_id"
            + $" ORDER BY observation.pub_date DESC LIMIT " + numberOfObs + " OFFSET " + numberOfObs * (page-1) +";";
        
        string query1 = "SELECT observation.*, user.* FROM observation, user, comment, proposal WHERE observation.author_id = user.user_id or (observation.observation_id = comment.observation_id or observation.observation_id = proposal.observation_id)"
            + $" ORDER BY observation.pub_date DESC LIMIT " + numberOfObs + " OFFSET " + numberOfObs * (page-1) +";";

        string query2 = "SELECT observation.*, user.* FROM observation INNER JOIN user ON observation.author_id = user.user_id INNER JOIN comment ON observation.observation_id = comment.observation_id INNER JOIN proposal ON observation.observation_id = proposal.observation_id"
        + $" ORDER BY observation.pub_date DESC LIMIT " + numberOfObs + " OFFSET " + numberOfObs * (page-1) +";";

        string query3 = "SELECT observation.*, user.* FROM observation, user, comment WHERE observation.author_id = user.user_id and observation_id = " + id
            + $" ORDER BY observation.pub_date DESC LIMIT " + numberOfObs + " OFFSET " + numberOfObs * (page-1) +";";*/
        
        /*_obs = dbf.getObservationCommentsProposals(queries);
        return _obs;*/

        List<ObservationViewModel> observations = new List<ObservationViewModel>();
        return observations; 
    }

    public static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }

}
