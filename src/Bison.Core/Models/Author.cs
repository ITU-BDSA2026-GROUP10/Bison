namespace Bison.Core.Models;
public class Author 
{
    public int AuthorId {get; set;}
    public string Name {get; set;}
    public string Email {get; set;}
    public ICollection<Post>? Posts {get; set;}
    //public ICollection<Post>? LikedPosts {get; set;}

}