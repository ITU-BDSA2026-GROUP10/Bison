public static class UserInterface
{
    public static void printObservations(IEnumerable<Cheep> obs)
    {
        foreach (var r in obs)
        {
            DateTime time = DateTimeOffset.FromUnixTimeSeconds(r.Timestamp).DateTime;
            if (typeof(Observation).IsInstanceOfType(r))
            {
                Observation observation = (Observation)r;
                Console.WriteLine(r.Author + " @ " + time + ": " + observation.observation.Trim('\"'));
            } else if (typeof(Comment).IsInstanceOfType(r))
            {
                Comment comment = (Comment)r;
                Console.WriteLine(r.Author + " @ " + time + ": " + comment.observationId);
            }
        }
    }
}