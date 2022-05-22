namespace Capstone1.Server.Services.PostService
{
    public class PostService : IPostService
    {
        private readonly DataContext _context;
        private readonly IAuthService _authService;

        public PostService(DataContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        #region "Get Methods"
        public async Task<ServiceResponse<List<PostInterest>>> GetAllPostInterests()
        {
            var postInterests = await _context.PostInterests.ToListAsync();
            if (postInterests == null)
            {
                new ServiceResponse<List<PostInterest>>
                {
                    Success = false,
                    Message = "No PostInterests found."
                };
            }

            return new ServiceResponse<List<PostInterest>> { Data = postInterests };
        }

        public async Task<ServiceResponse<List<Post>>> GetAllPosts()
        {
            List<Post> result = new List<Post>();
            int currentUserId = await _authService.GetUserId();
            if (currentUserId == 0)
                result = await _context.Posts.Where(p => !p.Deleted && p.Visible && !p.IsAccepted).ToListAsync();
            else
                result = await _context.Posts.Where(p => !p.Deleted && p.Visible && p.StudentId != currentUserId && !p.IsAccepted).ToListAsync();
            if (result == null || result.Count == 0)
            {
                return new ServiceResponse<List<Post>>
                {
                    Success = false,
                    Message = "There are no posts."
                };
            }

            return new ServiceResponse<List<Post>> { Data = result };
        }

        public async Task<ServiceResponse<List<Post>>> GetAllPostsWithAccepted()
        {
            var result = await _context.Posts.Where(p => !p.Deleted && p.Visible).ToListAsync();
            if (result == null || result.Count == 0)
            {
                return new ServiceResponse<List<Post>>
                {
                    Success = false,
                    Message = "There are no posts."
                };
            }

            return new ServiceResponse<List<Post>> { Data = result };
        }

        public async Task<ServiceResponse<List<Post>>> AdminGetPosts()
        {
            var result = await _context.Posts.Where(p => !p.Deleted).ToListAsync();
            if (result == null || result.Count == 0)
            {
                return new ServiceResponse<List<Post>>
                {
                    Success = false,
                    Message = "There are no posts."
                };
            }

            return new ServiceResponse<List<Post>> { Data = result };
        }

        public async Task<ServiceResponse<string>> GetNameOfPoster(int studentId)
        {
            var student = await _context.Users.FirstOrDefaultAsync(u => u.Id == studentId && !u.Deleted && u.Visible);
            if (student == null)
            {
                return new ServiceResponse<string>
                {
                    Success = false,
                    Message = "student not found."
                };
            }

            string fullName = student.FirstName + " " + student.LastName;
            return new ServiceResponse<string> { Data = fullName };
        }

        public async Task<ServiceResponse<Post>> GetPost(int id)
        {
            var response = new ServiceResponse<Post>();
            bool isAdmin = (await _authService.IsAdmin()).Data;
            Post post = null;

            post = await _context.Posts
                .Include(p => p.PostInterests)
                .ThenInclude(pi => pi.Interest)
                .FirstOrDefaultAsync(p => p.Id == id && !p.Deleted && (p.Visible || isAdmin));

            if (post == null)
            {
                response.Success = false;
                response.Message = "Sorry, but this post does not exist.";
            }
            else
            {
                response.Data = post;
            }

            return response;

        }

        public async Task<ServiceResponse<List<PostInterest>>> GetPostsInterests(int postId)
        {
            var result = await _context.PostInterests.Where(pi => pi.PostId == postId).ToListAsync();
            if (result == null)
            {
                return new ServiceResponse<List<PostInterest>>
                {
                    Success = false,
                    Message = "This post has no interests."
                };
            }

            return new ServiceResponse<List<PostInterest>> { Data = result };
        }

        private async Task<ServiceResponse<List<string>>> GetPostSearchSuggestionsByPostsAndUsers(string searchText, List<Post> allPosts, List<User> users)
        {
            var posts = await FindPostsBySearchText(searchText, allPosts);

            List<string> result = new List<string>();

            foreach (var user in users)
            {
                if ((user.FirstName.ToLower().Contains(searchText.ToLower()) || user.LastName.ToLower().Contains(searchText.ToLower())) && !result.Contains(user.FirstName + " " + user.LastName))
                {
                    result.Add(user.FirstName + " " + user.LastName);
                }
            }

            foreach (var post in posts)
            {
                if(!post.Deleted && post.Visible && !post.IsAccepted)
                {
                    if (post.Title.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                    {
                        result.Add(post.Title);
                    }

                    if (post.Description != null)
                    {
                        var punctuation = post.Description.Where(char.IsPunctuation)
                            .Distinct().ToArray();

                        var words = post.Description.Split().Select(s => s.Trim(punctuation));

                        foreach (var word in words)
                        {
                            if (word.Contains(searchText, StringComparison.OrdinalIgnoreCase)
                                && !result.Contains(word))
                            {
                                result.Add(word);
                            }
                        }
                    }
                }
            }

            var postsTags = await _context.Tags.ToListAsync();

            foreach (var tag in postsTags)
            {
                var tagsPost = await _context.Posts.FindAsync(tag.PostId);
                if (tag.Name.ToLower().Contains(searchText.ToLower()) && !result.Contains(tag.Name) && !tagsPost.Deleted && tagsPost.Visible && !tagsPost.IsAccepted)
                {
                    result.Add(tag.Name);
                }
            }

            return new ServiceResponse<List<string>> { Data = result };
        }

        public async Task<ServiceResponse<List<string>>> GetPostSearchSuggestions(string searchText)
        {
            var allPosts = await _context.Posts.Where(p => !p.Deleted && p.Visible && !p.IsAccepted).Include(p => p.PostInterests).ToListAsync();
            var users = await _context.Users.Where(u => !u.Deleted && u.Visible && u.Role.Equals("Student")).ToListAsync();

            return await GetPostSearchSuggestionsByPostsAndUsers(searchText, allPosts, users);
        }

        public async Task<ServiceResponse<List<string>>> AdminGetPostSearchSuggestions(string searchText)
        {
            var allPosts = await _context.Posts.Where(p => !p.Deleted).Include(p => p.PostInterests).ToListAsync();
            var users = await _context.Users.Where(u => !u.Deleted && u.Role.Equals("Student")).ToListAsync();

            return await GetPostSearchSuggestionsByPostsAndUsers(searchText, allPosts, users);
        }

        public async Task<ServiceResponse<PostDTO>> AdminSearchPosts(string searchText, int page)
        {
            var adminPosts = (await AdminGetPosts()).Data;
            return await SearchPosts(searchText, page, adminPosts);
        }

        public async Task<ServiceResponse<PostDTO>> AdminGetPostsPaginated(int page)
        {
            var pageResults = 5f;
            var result = (await AdminGetPosts()).Data.Skip((page - 1) * (int)pageResults).Take((int)pageResults).ToList();

            var response = new ServiceResponse<PostDTO>
            {
                Data = new PostDTO
                {
                    Posts = result,
                    CurrentPage = page,
                    Pages = (int)Math.Ceiling(await _context.Posts.Where(p => !p.Deleted).CountAsync() / pageResults)
                }
            };

            return response;
        }

        public async Task<ServiceResponse<string>> GetNameOfPosterByPostId(int postId)
        {
            var post = (await _context.Posts.FirstOrDefaultAsync(p => p.Id == postId && !p.Deleted && p.Visible));
            if (post == null)
            {
                return new ServiceResponse<string>
                {
                    Success = false,
                    Message = "This post doesn't exist."
                };
            }

            var student = await _context.Users.FirstOrDefaultAsync(u => u.Id == post.StudentId && !u.Deleted && u.Visible);
            if (student == null)
            {
                return new ServiceResponse<string>
                {
                    Success = false,
                    Message = "The student of this post doesn't exist."
                };
            }
            string fullName = student.FirstName + " " + student.LastName;
            return new ServiceResponse<string> { Data = fullName };
        }

        public async Task<ServiceResponse<List<Post>>> GetStudentsPostsPaginated(int studentId, int page)
        {
            var pageResults = 5f;
            bool isAdmin = (await _authService.IsAdmin()).Data;
            var postsPaginated = await _context.Posts.Where(p => p.StudentId == studentId && !p.Deleted && (!p.IsAccepted || isAdmin) && (p.Visible || isAdmin)).Skip((page - 1) * (int)pageResults).Take((int)pageResults).ToListAsync();
            if (postsPaginated == null || postsPaginated.Count == 0)
            {
                return new ServiceResponse<List<Post>>
                {
                    Success = false,
                    Message = "This student has no posts."
                };
            }

            return new ServiceResponse<List<Post>> { Data = postsPaginated };
        }

        public async Task<ServiceResponse<int>> GetStudentsPostsCount(int studentId)
        {
            bool isAdmin = (await _authService.IsAdmin()).Data;
            var postsCount = await _context.Posts.Where(p => p.StudentId == studentId && !p.Deleted && (!p.IsAccepted || isAdmin) && (p.Visible || isAdmin)).CountAsync();
            return new ServiceResponse<int> { Data = postsCount };
        }

        public async Task<ServiceResponse<List<Post>>> GetLoggedInStudentsPostsPaginated(int page)
        {
            var currentUserId = await _authService.GetUserId();
            bool isAdmin = (await _authService.IsAdmin()).Data;
            var pageResults = 5f;
            var postsPaginated = await _context.Posts.Where(p => p.StudentId == currentUserId && !p.Deleted && (p.Visible || isAdmin)).Skip((page - 1) * (int)pageResults).Take((int)pageResults).ToListAsync();
            if (postsPaginated == null || postsPaginated.Count == 0)
            {
                return new ServiceResponse<List<Post>>
                {
                    Success = false,
                    Message = "This student has no posts."
                };
            }

            return new ServiceResponse<List<Post>> { Data = postsPaginated };
        }

        public async Task<ServiceResponse<int>> GetLoggedInStudentsPostsCount()
        {
            var currentUserId = await _authService.GetUserId();
            bool isAdmin = (await _authService.IsAdmin()).Data;
            var postsCount = await _context.Posts.Where(p => p.StudentId == currentUserId && !p.Deleted && (p.Visible || isAdmin)).CountAsync();
            return new ServiceResponse<int> { Data = postsCount };
        }

        public async Task<ServiceResponse<string>> AdminGetPostTitle(int postId)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == postId && !p.Deleted);
            if (post == null)
            {
                return new ServiceResponse<string>
                {
                    Success = false,
                    Message = "Post not found to get its title."
                };
            }

            return new ServiceResponse<string> { Data = post.Title };
        }

        public async Task<ServiceResponse<Dictionary<int, string>>> GetPostsAuthors()
        {
            var posts = await _context.Posts.Where(p => !p.Deleted).ToListAsync();
            Dictionary<int, string> result = new Dictionary<int, string>();

            foreach (var post in posts)
            {
                var postsStudent = await _context.Users.FindAsync(post.StudentId);
                if (postsStudent != null)
                    result.Add(post.Id, postsStudent.FirstName + " " + postsStudent.LastName);
            }

            if (result == null || result.Count == 0)
            {
                return new ServiceResponse<Dictionary<int, string>>
                {
                    Success = false,
                    Message = "There hasn't been any posts to find the authors."
                };
            }

            return new ServiceResponse<Dictionary<int, string>> { Data = result };
        }

        public async Task<ServiceResponse<Dictionary<int, string>>> GetPostsAuthorsProfilePics()
        {
            var posts = await _context.Posts.Where(p => !p.Deleted).ToListAsync();
            Dictionary<int, string> result = new Dictionary<int, string>();

            foreach (var post in posts)
            {
                var postsStudent = await _context.Users.FindAsync(post.StudentId);
                if (postsStudent != null)
                    result.Add(post.Id, postsStudent.ProfilePic);
            }

            if (result == null || result.Count == 0)
            {
                return new ServiceResponse<Dictionary<int, string>>
                {
                    Success = false,
                    Message = "There hasn't been any posts to find the authors."
                };
            }

            return new ServiceResponse<Dictionary<int, string>> { Data = result };
        }

        public async Task<ServiceResponse<bool>> GetPostsIsAccepted(int postId)
		{
            var post = await _context.Posts.FindAsync(postId);
            if(post == null)
			{
                return new ServiceResponse<bool>
                {
                    Success = false,
                    Message = "Post not found to check if its been accepted."
                };
			}

            return new ServiceResponse<bool> { Data = post.IsAccepted };
		}
        #endregion

        #region "Post Methods"
        public async Task<ServiceResponse<Post>> AddPost(Post post)
        {
            post.StudentId = await _authService.GetUserId();
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
            return new ServiceResponse<Post> { Data = post };
        }
        public async Task<ServiceResponse<PostDTO>> GetPostsPagination(int page, List<Post> posts)
        {
            var pageResults = 5f;
            int? currentUserId = await _authService.GetUserId();
            double pageCount = 0;
            if (currentUserId == 0)
                pageCount = Math.Ceiling(await _context.Posts.Where(p => !p.Deleted && !p.IsAccepted && p.Visible).CountAsync() / pageResults);
            else
                pageCount = Math.Ceiling(await _context.Posts.Where(p => !p.Deleted && !p.IsAccepted && p.Visible && p.Id != currentUserId).CountAsync() / pageResults);
            var result = posts.Skip((page - 1) * (int)pageResults).Take((int)pageResults).ToList();
            var response = new ServiceResponse<PostDTO>
            {
                Data = new PostDTO
                {
                    Posts = result,
                    CurrentPage = page,
                    Pages = (int)pageCount
                }
            };

            return response;
        }
        public async Task<ServiceResponse<PostDTO>> SearchPosts(string searchText, int page, List<Post> FPosts)
        {
            var pageResults = 5f;
            var pageCount = Math.Ceiling((await FindPostsBySearchText(searchText, FPosts)).Count / pageResults);
            var posts = await FindPostsBySearchTextPagination(searchText, page, FPosts);

            var response = new ServiceResponse<PostDTO>
            {
                Data = new PostDTO
                {
                    Posts = posts,
                    CurrentPage = page,
                    Pages = (int)pageCount
                }
            };

            return response;
        }
        #endregion

        #region "Put Methods"
        public async Task<ServiceResponse<Post>> UpdatePost(Post post)
        {
            bool isAdmin = (await _authService.IsAdmin()).Data;
            var dbPost = await _context.Posts.FirstOrDefaultAsync(p => p.Id == post.Id && !p.Deleted && (p.Visible || isAdmin));
            if(dbPost == null)
            {
                return new ServiceResponse<Post>
                {
                    Success = false,
                    Message = "Post not found to update."
                };
            }

            dbPost.Title = post.Title;
            dbPost.Description = post.Description;
            dbPost.Budget = post.Budget;
            var dbStudent = await _context.Users.FirstOrDefaultAsync(u => u.Id == dbPost.StudentId);
            if(dbPost.Visible.CompareTo(post.Visible) != 0 && dbStudent.Visible)
            {
                dbPost.Visible = post.Visible;
                var proposals = await _context.Proposals.Where(p => p.PostId == dbPost.Id && !p.Deleted).ToListAsync();
                foreach (var proposal in proposals)
                {
                    var dbTeacher = await _context.Users.FirstOrDefaultAsync(u => u.Id == proposal.TeacherId);
                    if (dbTeacher != null && dbTeacher.Visible && !dbPost.Visible)
                    {
                        var dbProposal = await _context.Proposals.FirstOrDefaultAsync(p => p.Id == proposal.Id);
                        if (dbProposal != null)
                            dbProposal.Visible = post.Visible;
                    }
                }
            }

			for (int i = 0; i < post.PostInterests.Count; i++)
			{
				for (int j = i + 1; j < post.PostInterests.Count; j++)
				{
                    if(post.PostInterests[i].InterestId == post.PostInterests[j].InterestId)
					{
                        return new ServiceResponse<Post>
                        {
                            Success = false,
                            Message = "You have selected the same interest more than once."
                        };
					}
				}
			}

            List<PostInterest> postsInterests = await _context.PostInterests.Where(pi => pi.PostId == post.Id).ToListAsync();
			foreach (var postInterest in postsInterests)
			{
                _context.PostInterests.Remove(postInterest);
			}

            await _context.SaveChangesAsync();

            foreach (var postInterest in post.PostInterests)
            {
                postInterest.PostId = post.Id;
                postInterest.Interest = null;
                _context.PostInterests.Add(postInterest);
            }

            await _context.SaveChangesAsync();
            return new ServiceResponse<Post> { Data = post };
        }
        #endregion

        #region "Delete Methods"
        public async Task<ServiceResponse<Post>> DeletePost(int postId)
        {
            bool isAdmin = (await _authService.IsAdmin()).Data;
            var dbPost = await _context.Posts.FirstOrDefaultAsync(p => p.Id == postId && !p.Deleted && (p.Visible || isAdmin));
            if (dbPost == null)
            {
                return new ServiceResponse<Post>
                {
                    Success = false,
                    Message = "Post not found to delete."
                };
            }

            dbPost.Deleted = true;
            var proposals = await _context.Proposals.Where(p => p.PostId == postId && !p.Deleted).ToListAsync();
            foreach (var proposal in proposals)
            {
                var dbProposal = await _context.Proposals.FirstOrDefaultAsync(p => p.Id == proposal.Id);
                if (dbProposal != null)
                    dbProposal.Deleted = true;
            }
            await _context.SaveChangesAsync();
            return new ServiceResponse<Post> { Data = dbPost };
        }
        #endregion

        #region "Private Methods"
        private async Task<List<Post>> FindPostsBySearchText(string searchText, List<Post> posts)
        {
            var tags = await _context.Tags.ToListAsync();
            int? currentUserId = await _authService.GetUserId();
            List<User> users = new List<User>();
            var result = new List<Post>();
            if (currentUserId == 0)
            {
                users = await _context.Users.Where(u => !u.Deleted && u.Visible && u.Role.Equals("Student")).ToListAsync();
                result.AddRange(posts
                        .Where(p => (p.Title.ToLower().Contains(searchText.ToLower())
                         ||
                         p.Description.ToLower().Contains(searchText.ToLower())) && !p.Deleted && p.Visible && !p.IsAccepted).ToList());
            }
            else
            {
                users = await _context.Users.Where(u => !u.Deleted && u.Visible && u.Role.Equals("Student") && u.Id != currentUserId).ToListAsync();
                result.AddRange(posts
                        .Where(p => (p.Title.ToLower().Contains(searchText.ToLower())
                         ||
                         p.Description.ToLower().Contains(searchText.ToLower())) && p.StudentId != currentUserId).ToList());
            }
            foreach (var post in posts)
            {
                var postsTags = tags.Where(t => t.PostId == post.Id).ToList();
                if (postsTags.Count != 0)
                {
                    foreach (var tag in postsTags)
                    {
                        if (tag.Name.ToLower().Contains(searchText.ToLower()) && !result.Contains(post) && !post.Deleted && post.Visible && !post.IsAccepted)
                        {
                            result.Add(post);
                        }
                    }
                }

                foreach (var user in users)
                {
                    if ((user.FirstName.ToLower().Contains(searchText.ToLower()) || user.LastName.ToLower().Contains(searchText.ToLower()) || (user.FirstName + " " + user.LastName).ToLower().Contains(searchText.ToLower())) && !result.Contains(post))
                    {
                        if (user.Id == post.StudentId)
                            result.Add(post);
                    }
                }

            }

            return result;
        }

        private async Task<List<Post>> FindPostsBySearchTextPagination(string searchText, int page, List<Post> posts)
        {
            var pageResults = 5f;
            var result = await FindPostsBySearchText(searchText, posts);
            return result.Skip((page - 1) * (int)pageResults).Take((int)pageResults).ToList();
        }
        #endregion
    }
}
