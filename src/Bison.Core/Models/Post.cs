using System.Globalization;

namespace Bison.Core.Models;

public class Post
{
    public int PostId { get; set; }
    public Author Author { get; set; }
    public string? Text { get; set; }
    public DateTime TimeStamp { get; set; }
    public int AuthorId { get; set; }

    public Post(Author author, string text, int postid)
    {
        PostId = postid;
        Author = author;
        AuthorId = author.AuthorId;
        Text = text;
        TimeStamp = DateTime.Now;
    }

    public Post() { }
}