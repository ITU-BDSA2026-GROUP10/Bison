public static class UserInterface
{
    public static void printObservations(IEnumerable<Cheep> obs)
    {
        foreach (var r in obs)
        {
            DateTime time = DateTimeOffset.FromUnixTimeSeconds(r.Timestamp).DateTime;
            Console.WriteLine(r.Author + " @ " + time + ": " + r.Observation.Trim('\"'));
        }
    }

    public static void printDiscussion(IEnumerable<Comment> obs)
    {
        foreach (var r in obs)
        {
            DateTime time = DateTimeOffset.FromUnixTimeSeconds(r.Timestamp).DateTime;
            Console.WriteLine(r.Author + " @ " + time + ": " + r.Observation.Trim('\"'));
        }
    }
}