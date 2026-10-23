namespace DataModel;
public class Post
{
    public int PostId {get; set;}
    public Author Author {get; set;}
    public string Text {get; set;} = "";
    public DateTime TimeStamp {get; set;}
    public int AuthorId { get; set; }
}