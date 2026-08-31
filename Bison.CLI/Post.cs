using System;
using System.Collections.Specialized;

public class Post
{
    private string author;
    private long timecode;
    private string observation;

    Post(string authors, string observations, long timecodes)
    {
        author = authors;
        observation = observations;
        timecode = timecodes;
    }

    public string GetAuthor()
    {
        return author;
    }
    public long GetTime()
    {
        return timecode;
    }
    public string GetObservation()
    {
        return observation;
    }
    /*long GetTime()
    {
        return DateTimeOffset.FromUnixTimeSeconds(timecode).DateTime;
    }*/
}