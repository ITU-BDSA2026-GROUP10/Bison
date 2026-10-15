namespace DataModel;
public class Post : Author
{
    public int PostID {get; set;}
    public string Text {get; set;}
    public DateTime Timestamp {get; set;}
    public Author Author {get; set;}
}