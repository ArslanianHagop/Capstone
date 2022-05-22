namespace Capstone1.Client.Services.PostService
{
    public interface IPostService
    {
        event Action PostsChanged;
        Task PostsChangedInvoked();
        #region "Lists"
        List<Post> Posts { get; set; }
        List<Post> PostsPaged { get; set; }
        List<Post> FilteredPosts { get; set; }
        List<Post> AdminPosts { get; set; }
        #endregion
        string Message { get; set; }
        int CurrentPage { get; set; }
        int PageCount { get; set; }
        string LastSearchText { get; set; }

        #region "Get Methods"
        Task<List<Post>> GetAllPosts();
        Task<List<Post>> GetAllPostsWithAccepted();
        Task<ServiceResponse<Post>> GetPost(int id);
        Task AdminGetPosts();
        Task AdminGetPostsPaginated(int page);
        Task GetPosts();
        Task<string> GetNameOfPoster(int studentId);
        Task<string> GetNameOfPosterByPostId(int postId);
        Task<List<PostInterest>> GetPostsInterests(int postId);
        Task<List<PostInterest>> GetAllPostInterests();
        Task FilterPosts(List<Interest> interests);
        Task SearchPosts(string searchText, int page, List<Post> posts);
        Task AdminSearchPosts(string searchText, int page);
        Task<List<string>> GetPostSearchSuggestions(string searchText);
        Task<List<string>> AdminGetPostSearchSuggestions(string searchText);
        Task<List<Post>> GetStudentsPostsPaginated(int studentId, int page);
        Task<int> GetStudentsPostsCount(int studentId);
        Task<List<Post>> GetLoggedInStudentsPostsPaginated(int page);
        Task<int> GetLoggedInStudentsPostsCount();
        Task<string> AdminGetPostTitle(int postId);
        Task<Dictionary<int, string>> GetPostsAuthors();
        Task<Dictionary<int, string>> GetPostsAuthorsProfilePics();
        Task<bool> GetPostsIsAccepted(int postId);
        #endregion
        #region "Post Methods"
        Task<Post> AddPost(Post post);
        Task GetPostsPagination(int page, List<Post> posts);
        Task<List<Post>> SearchPostsReturn(string searchText, int page, List<Post> posts);
        #endregion

        #region "Put Methods"
        Task<ServiceResponse<Post>> UpdatePost(Post post);
        #endregion
        #region "Delete Methods"
        Task DeletePost(Post post);
        #endregion
    }
}
