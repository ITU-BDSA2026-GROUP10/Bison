
public class PostDTO : AuthorDTO
{
    public int PostID {get; set;}
    public string Text {get; set;}
    public string Timestamp {get; set;}
    public AuthorDTO Author {get; set;}
    public string AuthorName {get; set;}
}