
using SimpleDB;
public static class UserInterface
{
    public static void printObservations(IEnumerable<Observations> obs)
    {
        foreach (var r in obs)
        {
            DateTime time = GetDateTime(r.Timestamp);
            Console.WriteLine(r.Author + " @ " + time + ": " + r.Observation.Trim('\"'));
        }
    }

    public static void printDiscussion(long id, IEnumerable<Comment> obs)
    {
        foreach (var r in obs)
        {
            if (r.ObservationId == id) 
            {
                DateTime time = GetDateTime(r.Timestamp);
                Console.WriteLine(r.Author + " @ " + time + ": " + r.Observation.Trim('\"'));
            }
        } 
    }

    public static void printObservationsByLocation(string location, IEnumerable<Observations> obs)
    {    
        foreach (var r in obs)
        {
            if (r.Location.Equals(location, StringComparison.OrdinalIgnoreCase)){
                DateTime time = DateTimeOffset.FromUnixTimeSeconds(r.Timestamp).DateTime;
                Console.WriteLine(r.Author + " @ " + time + ": " + r.Observation.Trim('\"'));
            }
            /*ignore case: https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/strings/common-tasks/compare
            */
        }
    }
    public static DateTime GetDateTime(long timestamp)
    {
        return DateTimeOffset.FromUnixTimeSeconds(timestamp).DateTime;
    }

    public static bool ObservationExists(long count, long ObservationID)
    {
        return count >= ObservationID;
    }
}