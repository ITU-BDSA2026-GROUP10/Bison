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
}