namespace DataModel;
using System.Globalization;
public class Post
{
    public int PostId {get; set;}
    public Author Author {get; set;}
    public string Text {get; set;} = "";
    public DateTime TimeStamp {get; set;}
    public int AuthorId { get; set; }
    
    public Post (Author author, string text, int postid){
        PostId = postid;
        Author = author;
        Text = text;
        TimeStamp = DateTime.Now;
    }
    
    public Post () {}
}