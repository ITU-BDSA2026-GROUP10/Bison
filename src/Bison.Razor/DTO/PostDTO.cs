namespace DTO;
public class PostDTO : AuthorDTO
{
    public int PostId {get; set;}
    public string Text {get; set;}
    public string TimeStamp {get; set;}
    public AuthorDTO Author {get; set;}
    public string AuthorName {get; set;}
}