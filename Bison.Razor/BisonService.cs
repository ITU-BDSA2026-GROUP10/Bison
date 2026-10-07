public record ObservationViewModel(string Author, string Message, string Timestamp);

public interface IObservationService
{
    public List<ObservationViewModel> GetObservations(int page);
    public List<ObservationViewModel> GetObservationsFromAuthor(string author);

}

public class ObservationService : IObservationService
{
    DBFacade dbf = new DBFacade();
    // These would normally be loaded from a database for example
    private /*readonly*/List<ObservationViewModel> _obs;

    public List<ObservationViewModel> GetObservations(int page)
    {
        int numberOfObs = 32;
        _obs = 
            dbf.getObservationsFromDatabase("SELECT observation.*, user.* FROM observation, user WHERE observation.author_id = user.user_id"
            + $" ORDER BY observation.pub_date DESC LIMIT " + numberOfObs + " OFFSET " + numberOfObs * page +";");
        return _obs;
    }

    public List<ObservationViewModel> GetObservationsFromAuthor(string author)
    {
        // filter by the provided author name
        return _obs.Where(x => x.Author == author).ToList();
    }

    public static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }

}
