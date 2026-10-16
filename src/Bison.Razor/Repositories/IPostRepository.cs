namespace Bison.Razor.PostRepository;

using DataModel;

public interface IPostRepository
{
    public Task<List<PostDTO>> ReadPostsByAuthor(string author, int page);
    public Task<List<PostDTO>> ReadPosts(int page);
    public Task<List<PostDTO>> ReadPost(int PostID);
}