namespace Bison.Core.Models;

public class Comment : Post
{
    public Observation Observation {get; set;}
    
    
    public Comment () {}
    
    public Comment (Author a, string text, int postid, Observation obs)
    {
        Author = a;
        AuthorId = a.AuthorId;
        Text = text;
        PostId = postid;
        Observation = obs;
    }
}