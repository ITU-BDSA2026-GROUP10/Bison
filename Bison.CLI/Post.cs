using System;
using System.Collections.Specialized;
using System.Net;

public class Post
{
    string author;
    long timecode;
    string observation;

    public Post(string authors, string observations, string timecodes)
    {
        author = authors;
        observation = observations;
        timecode = long.Parse(timecodes);
        //potential bug with parse?
    }

    public string getAuthor ()
    {
        return author;
    }

    public string getObservation()
    {
        return observation;
    }

    public long getTimecode()
    {
        return timecode;
    }
}