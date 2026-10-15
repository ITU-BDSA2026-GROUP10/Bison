using DataModel;

public interface IPostRepository
{
    public Task<List<PostDTO>> ReadPostsByAuthor(int AuthorID);
    public Task<List<PostDTO>> ReadPosts(int page);
    public Task<List<PostDTO>> ReadPost(int PostID);
}