
public class PostDTO : AuthorDTO
{
    public int PostID {get; set;}
    public string Text {get; set;}
    public DateTime Timestamp {get; set;}
    public AuthorDTO Author {get; set;}
}