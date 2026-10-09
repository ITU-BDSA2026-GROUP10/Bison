public record ObservationViewModel(string Author, string Message, string Timestamp);

public interface IObservationService
{
    public List<ObservationViewModel> GetObservations(int page);
    public List<ObservationViewModel> GetObservationsFromAuthor(string author, int page);
    
    public List<ObservationViewModel> GetObservationFromID(int id, int page);
}

public class ObservationService : IObservationService
{
    DBFacade dbf = new DBFacade();
    // These would normally be loaded from a database for example
    private /*readonly*/List<ObservationViewModel> _obs;

    public List<ObservationViewModel> GetObservations(int page)
    {
        int numberOfObs = 32;

        if (page == 0) 
        {
            page = 1;
        }

        _obs = 
            dbf.getObservationsFromDatabase("SELECT observation.*, user.* FROM observation, user WHERE observation.author_id = user.user_id"
            + $" ORDER BY observation.pub_date DESC LIMIT " + numberOfObs + " OFFSET " + numberOfObs * (page-1) +";");
        return _obs;
    }

    public List<ObservationViewModel> GetObservationsFromAuthor(string author, int page)
    {
        // filter by the provided author name

        int numberOfObs = 32;

        if (page == 0) 
        {
            page = 1;
        }

        _obs = 
            dbf.getObservationsFromDatabase("SELECT observation.*, user.* FROM observation, user WHERE observation.author_id = user.user_id" + 
            " AND user.username = @author" + 
            $" ORDER BY observation.pub_date DESC LIMIT " + numberOfObs + " OFFSET " + numberOfObs * (page-1) +";", author);
        return _obs;

        //return _obs.Where(x => x.Author == author).ToList();
    }

    public List<ObservationViewModel> GetObservationFromID(int id, int page)
    {
        int numberOfObs = 32;

        if (id == 0) 
        {
            return GetObservations(1);
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
        queries[1] = "SELECT observation.*, user.* FROM observation, user, comment WHERE observation.author_id = user.user_id and observation.observation_id = " + id + " and observation.observation_id = comment.observation_id"
            + $" ORDER BY observation.pub_date DESC LIMIT " + numberOfObs + " OFFSET " + numberOfObs * (page-1) +";";

        queries[2] = "SELECT observation.*, user.* FROM observation, user, proposal WHERE observation.author_id = user.user_id and observation.observation_id = " + id + " and observation.observation_id = proposal.observation_id"
            + $" ORDER BY observation.pub_date DESC LIMIT " + numberOfObs + " OFFSET " + numberOfObs * (page-1) +";";

        _obs = dbf.getObservationCommentsProposals(queries);
        return _obs;
    }

    public static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }

}
