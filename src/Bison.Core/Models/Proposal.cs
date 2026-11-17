namespace Bison.Core.Models;

public class Proposal : Post
{
    public Observation Observation {get; set;}
    public Taxon Taxon {get; set;}
    
    
    public Proposal () {}
    
    public Proposal (Author a, string text, int postid, Observation ob, Taxon tax)
    {
        Author = a;
        AuthorId = a.AuthorId;
        Text = text;
        PostId = postid;
        Observation = ob;
        Taxon = tax;
    }
}