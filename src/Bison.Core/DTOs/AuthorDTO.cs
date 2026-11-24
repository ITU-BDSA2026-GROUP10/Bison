namespace Bison.Core.DTOs;
public class AuthorDTO
{
    public int AuthorID {get; set;}
    public string Name {get; set;}
    public string Email {get; set;}
    public ICollection<PostDTO>? Posts {get; set;}
    public IList<int>? FavoritePosts {get; set;}
}