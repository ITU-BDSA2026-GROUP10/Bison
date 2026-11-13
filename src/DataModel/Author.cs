namespace DataModel;
public class Author 
{
    public int AuthorId {get; set;}
    public string Name {get; set;}
    public string Email {get; set;}
    public ICollection<Post>? Posts {get; set;}

    public Author(string name, string email)
    {
        Name = name;
        Email = email;
    }
    
    public Author () {}
}