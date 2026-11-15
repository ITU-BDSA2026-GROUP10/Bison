using System.Diagnostics;
using Microsoft.AspNetCore.Authentication;

namespace DataModel;
public class Observation : Post
{
    public Taxon Taxon {get; set;}

    public List<Comment>? Comments {get; set;}

    public List<Proposal>? Proposals {get; set;}

    public Taxon getTaxon()
    {
        return Taxon;
    }
    public Observation (Author author, string text, int PostId) {
        Author = author;
        text = text;
        PostId = PostId;
    }
}