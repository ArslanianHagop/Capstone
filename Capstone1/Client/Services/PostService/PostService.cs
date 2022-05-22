namespace Capstone1.Client.Services.PostService
{
    public class PostService : IPostService
    {
        private readonly HttpClient _http;

        public PostService(HttpClient http)
        {
            _http = http;
        }

        #region "Lists"
        public List<Post> Posts { get; set; }
        public List<Post> PostsPaged { get; set; }
        public List<Post> FilteredPosts { get; set; }
        public List<Post> AdminPosts { get; set; }
        #endregion

        public string Message { get; set; } = "Loading Posts...";
        public int CurrentPage { get; set; } = 1;
        public int PageCount { get; set; } = 0;
        public string LastSearchText { get; set; } = string.Empty;

        public event Action PostsChanged;

        #region "Get Methods"
        public async Task FilterPosts(List<Interest> interests)
        {
            var pageResults = 5;
            FilteredPosts = new List<Post>();
            List<int> interestsIds = new List<int>();
            foreach (var interest in interests)
            {
                if (interest.IsSelected)
                {
                    interestsIds.Add(interest.Id);
                }
            }
            List<Post> allPosts = await GetAllPosts();
            if (interestsIds.Count > 0)
            {
                foreach (var post in allPosts)
                {
                    var postsInterests = await GetPostsInterests(post.Id);
                    List<int> postsInterestsIds = new List<int>();
                    foreach (var postsInterest in postsInterests)
                    {
                        postsInterestsIds.Add(postsInterest.InterestId);
                    }
                    bool isSubset = true;
                    foreach (var interestId in interestsIds)
                    {
                        if (!postsInterestsIds.Contains(interestId))
                        {
                            isSubset = false;
                            break;
                        }
                    }

                    if (isSubset)
                    {
                        FilteredPosts.Add(post);
                    }
                }
            }

            if (FilteredPosts.Count > 0)
            {
                PageCount = (int)Math.Ceiling((double)FilteredPosts.Count / pageResults);
            }
            else if (FilteredPosts.Count == 0)
            {
                if (LastSearchText.Equals(""))
                {
                    PageCount = (int)Math.Ceiling((double)(await GetAllPosts()).Count / pageResults);
                }
                else
                {
                    PageCount = (int)Math.Ceiling((double)(await SearchPostsReturn(LastSearchText, CurrentPage, await GetAllPosts())).Count / pageResults);
                }
            }

            PostsChanged?.Invoke();
        }

        public async Task<List<PostInterest>> GetAllPostInterests()
        {
            var result = await _http
                .GetFromJsonAsync<ServiceResponse<List<PostInterest>>>("api/post/postInterests");
            return result.Data;
        }

        public async Task GetPosts()
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<Post>>>("api/post/all");
            if (result != null && result.Data != null)
            {
                Posts = result.Data;
            }

            CurrentPage = 1;
            PageCount = 0;

            if (Posts.Count == 0)
                Message = "No Posts Found.";

            PostsChanged?.Invoke();
        }

        public async Task AdminGetPosts()
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<Post>>>("api/post/admin/all");
            if (result != null && result.Data != null)
            {
                AdminPosts = result.Data;
            }

            PostsChanged?.Invoke();
        }

        public async Task<List<Post>> GetAllPosts()
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<Post>>>("api/post/all");
            if (result != null && result.Data != null)
            {
                return result.Data;
            }
            else
            {
                Message = "No posts were found.";
                return new List<Post>();
            }
        }

        public async Task<List<Post>> GetAllPostsWithAccepted()
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<Post>>>("api/post/allWithAccepted");
            return result.Data;
        }

        public async Task<string> GetNameOfPoster(int studentId)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<string>>($"api/post/name-{studentId}");
            return result.Data;
        }

        public async Task<ServiceResponse<Post>> GetPost(int id)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<Post>>($"api/post/{id}");
            return result;
        }

        public async Task<List<string>> GetPostSearchSuggestions(string searchText)
        {
            var result = await _http
                .GetFromJsonAsync<ServiceResponse<List<string>>>($"api/post/searchsuggestions/{searchText}");
            return result.Data;
        }

        public async Task<List<string>> AdminGetPostSearchSuggestions(string searchText)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<string>>>($"api/post/admin/searchsuggestions/{searchText}");
            return result.Data;
        }

        public async Task<List<PostInterest>> GetPostsInterests(int postId)
        {
            var result = await _http
                .GetFromJsonAsync<ServiceResponse<List<PostInterest>>>($"api/post/interests-{postId}");
            return result.Data;
        }

        public async Task AdminSearchPosts(string searchText, int page)
        {
            LastSearchText = searchText;
            var result = await _http
                .GetFromJsonAsync<ServiceResponse<PostDTO>>($"api/post/admin/search/{searchText}/{page}");
            if (result != null && result.Data != null)
            {
                AdminPosts = result.Data.Posts;
                CurrentPage = result.Data.CurrentPage;
                PageCount = result.Data.Pages;
            }

            if (AdminPosts.Count == 0)
                Message = "No Posts found.";

            PostsChanged?.Invoke();
        }

        public async Task AdminGetPostsPaginated(int page)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<PostDTO>>($"api/post/admin/{page}");
            if (result != null && result.Data != null)
            {
                AdminPosts = result.Data.Posts;
                CurrentPage = page;
                PageCount = result.Data.Pages;
            }

            PostsChanged.Invoke();
        }

        public async Task<string> GetNameOfPosterByPostId(int postId)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<string>>($"api/post/fullName-{postId}");
            return result.Data;
        }

        public async Task<List<Post>> GetStudentsPostsPaginated(int studentId, int page)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<Post>>>($"api/post/studentsPostsPaginated/{studentId}/{page}");
            return result.Data;
        }

        public async Task<int> GetStudentsPostsCount(int studentId)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<int>>($"api/post/studentsPostsCount/{studentId}");
            return result.Data;
        }

        public async Task<List<Post>> GetLoggedInStudentsPostsPaginated(int page)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<Post>>>($"api/post/loggedInStudentsPostsPaginated/{page}");
            return result.Data;
        }

        public async Task<int> GetLoggedInStudentsPostsCount()
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<int>>("api/post/loggedInStudentsPostsCount");
            return result.Data;
        }

        public async Task<string> AdminGetPostTitle(int postId)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<string>>($"api/post/admin/title/{postId}");
            return result.Data;
        }

        public async Task<Dictionary<int, string>> GetPostsAuthors()
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<Dictionary<int, string>>>("api/post/authors");
            return result.Data;
        }

        public async Task<Dictionary<int, string>> GetPostsAuthorsProfilePics()
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<Dictionary<int, string>>>("api/post/authorsProfilePic");
            return result.Data;
        }

        public async Task<bool> GetPostsIsAccepted(int postId)
		{
            var result = await _http.GetFromJsonAsync<ServiceResponse<bool>>($"api/post/postsIsAccepted/{postId}");
            return result.Data;
		}
        #endregion

        #region "Post Methods"
        public async Task<Post> AddPost(Post post)
        {
            var result = await _http.PostAsJsonAsync("api/post", post);
            return (await result.Content.ReadFromJsonAsync<ServiceResponse<Post>>()).Data;
        }

        public async Task SearchPosts(string searchText, int page, List<Post> posts)
        {
            LastSearchText = searchText;
            var result = await _http
                .PostAsJsonAsync($"api/post/search/{searchText}/{page}", posts);
            var resultData = (await result.Content.ReadFromJsonAsync<ServiceResponse<PostDTO>>()).Data;
            if (result != null && resultData != null)
            {
                Posts = resultData.Posts;
                CurrentPage = resultData.CurrentPage;
                PageCount = resultData.Pages;
            }

            if (Posts.Count == 0)
                Message = "No Posts found.";

            PostsChanged?.Invoke();
        }

        public async Task GetPostsPagination(int page, List<Post> posts)
        {
            var result = await _http.PostAsJsonAsync($"api/post/{page}", posts);
            var resultData = (await result.Content.ReadFromJsonAsync<ServiceResponse<PostDTO>>()).Data;
            if (result != null && resultData != null)
            {
                Posts = resultData.Posts;
                CurrentPage = page;
                PageCount = resultData.Pages;
            }

            if (Posts.Count == 0)
                Message = "No Posts found.";

            PostsChanged?.Invoke();
        }

        public async Task<List<Post>> SearchPostsReturn(string searchText, int page, List<Post> posts)
        {
            if (posts.Count > 0)
            {
                LastSearchText = searchText;
                var result = await _http
                    .PostAsJsonAsync($"api/post/search/{searchText}/{page}", posts);
                var resultData = (await result.Content.ReadFromJsonAsync<ServiceResponse<PostDTO>>()).Data;
                if (result != null && resultData != null)
                {
                    CurrentPage = resultData.CurrentPage;
                    PageCount = resultData.Pages;
                    return resultData.Posts;
                }
                else
                {
                    return new List<Post>();
                }
            }
            else
            {
                return new List<Post>();
            }
        }
        #endregion

        #region "Delete Methods"
        public async Task DeletePost(Post post)
        {
            var result = await _http.DeleteAsync($"api/post/{post.Id}");
        }
        #endregion

        #region "Put Methods"
        public async Task<ServiceResponse<Post>> UpdatePost(Post post)
        {
            var result = await _http.PutAsJsonAsync("api/post", post);
            return (await result.Content.ReadFromJsonAsync<ServiceResponse<Post>>());
        }
        #endregion

        public async Task PostsChangedInvoked()
        {
            PostsChanged?.Invoke();
        }
    }
}
