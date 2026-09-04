using System;
using System.Collections.Specialized;
using System.Net;

public record Cheep
{
    public Cheep(string Author, string observations, string timecodes)
    {
        author = author;
        observation = observations;
        timecode = long.Parse(timecodes);
        //potential bug with parse?
        //hello this is test branch
    }
}