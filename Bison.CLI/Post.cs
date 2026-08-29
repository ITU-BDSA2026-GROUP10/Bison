using System;
using System.Collections.Specialized;
using System.Net;

class Post
{
    string author;
    long timecode;
    string observation;

    Post(string authors, string observations, string timecodes)
    {
        author = authors;
        observation = observations;
        timecode = long.Parse(timecodes);
        //potential bug with parse?
    }

    public string getAuthors ()
    {
        return author;
    }
}