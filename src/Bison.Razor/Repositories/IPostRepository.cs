namespace Bison.Razor.PostRepository;

using DataModel;

public interface IPostRepository
{
    public Task<List<PostDTO>> ReadPostsByAuthor(int AuthorID);
    public Task<List<PostDTO>> ReadPosts();
    public Task<List<PostDTO>> ReadPost(int PostID);
}