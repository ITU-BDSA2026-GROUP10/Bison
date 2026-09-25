
using SimpleDB;
public static class UserInterface
{
    public static void printObservations(IEnumerable<Observations> obs)
    {
        foreach (var r in obs)
        {
            Console.WriteLine(r.Author + " @ " + convertTime(r.Timestamp) + ": " + r.Observation.Trim('\"'));
        }
    }

    public static void printDiscussion(long id, IEnumerable<Comment> obs)
    {
        foreach (var r in obs)
        {
            if (r.ObservationId == id) 
            {
                Console.WriteLine(r.Author + " @ " + convertTime(r.Timestamp) + ": " + r.Observation.Trim('\"'));
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

    public static DateTime convertTime(long UnixTime)
    {
        DateTime time = DateTimeOffset.FromUnixTimeSeconds(UnixTime).DateTime;
        return time;
    }
}