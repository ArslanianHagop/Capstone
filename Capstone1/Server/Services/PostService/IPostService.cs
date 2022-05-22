namespace Capstone1.Server.Services.PostService
{
    public interface IPostService
    {
        #region "Get Methods"
        Task<ServiceResponse<List<Post>>> GetAllPosts();
        Task<ServiceResponse<List<Post>>> GetAllPostsWithAccepted();
        Task<ServiceResponse<List<Post>>> AdminGetPosts();
        Task<ServiceResponse<PostDTO>> AdminGetPostsPaginated(int page);
        Task<ServiceResponse<List<PostInterest>>> GetPostsInterests(int postId);
        Task<ServiceResponse<List<PostInterest>>> GetAllPostInterests();
        Task<ServiceResponse<Post>> GetPost(int id);
        Task<ServiceResponse<string>> GetNameOfPoster(int studentId);
        Task<ServiceResponse<string>> GetNameOfPosterByPostId(int postId);
        Task<ServiceResponse<PostDTO>> AdminSearchPosts(string searchText, int page);
        Task<ServiceResponse<List<string>>> GetPostSearchSuggestions(string searchText);
        Task<ServiceResponse<List<string>>> AdminGetPostSearchSuggestions(string searchText);
        Task<ServiceResponse<List<Post>>> GetStudentsPostsPaginated(int studentId, int page);
        Task<ServiceResponse<int>> GetStudentsPostsCount(int studentId);
        Task<ServiceResponse<List<Post>>> GetLoggedInStudentsPostsPaginated(int page);
        Task<ServiceResponse<int>> GetLoggedInStudentsPostsCount();
        Task<ServiceResponse<string>> AdminGetPostTitle(int postId);
        Task<ServiceResponse<Dictionary<int, string>>> GetPostsAuthors();
        Task<ServiceResponse<Dictionary<int, string>>> GetPostsAuthorsProfilePics();
        Task<ServiceResponse<bool>> GetPostsIsAccepted(int postId);
        #endregion
        #region "Post Methods"
        Task<ServiceResponse<Post>> AddPost(Post post);
        Task<ServiceResponse<PostDTO>> SearchPosts(string searchText, int page, List<Post> posts);
        Task<ServiceResponse<PostDTO>> GetPostsPagination(int page, List<Post> posts);
        #endregion
        #region "Put Methods"
        Task<ServiceResponse<Post>> UpdatePost(Post post);
        #endregion
        #region "Delete Methods"
        Task<ServiceResponse<Post>> DeletePost(int postId);
        #endregion
    }
}
