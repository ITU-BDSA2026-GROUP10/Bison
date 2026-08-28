using System;
using System.Collections.Specialized;

class Post
{
    string author;
    string timecode;
    string observation;

    Post(string authors, string observations, string timecodes)
    {
        author = authors;
        observation = observations;
        timecode = timecodes;
    }
}