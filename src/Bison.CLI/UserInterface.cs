public static class UserInterface
{
    public static void printObservations(IEnumerable<Observation> obs)
    {
        foreach (var r in obs)
        {
            DateTime time = DateTimeOffset.FromUnixTimeSeconds(r.Timestamp).DateTime;
            Console.WriteLine(r.Author + " @ " + time + ": " + r.Observation.Trim('\"'));
        }
    }

    public static void printDiscussion(long id, IEnumerable<Comment> obs)
    {
        foreach (var r in obs)
        {
            if (r.ObservationId == id) 
            {
                DateTime time = DateTimeOffset.FromUnixTimeSeconds(r.Timestamp).DateTime;
                Console.WriteLine(r.Author + " @ " + time + ": " + r.Observation.Trim('\"'));
            }
        } 
    }

    public static void printObservationsByLocation(string location, IEnumerable<Observation> obs)
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
}