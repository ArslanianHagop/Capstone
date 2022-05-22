using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Capstone1.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        #region "Get Methods"
        [HttpGet("all")]
        public async Task<ActionResult<ServiceResponse<List<Post>>>> GetAllPosts()
        {
            var response = await _postService.GetAllPosts();
            return Ok(response);
        }

        [HttpGet("allWithAccepted")]
        public async Task<ActionResult<ServiceResponse<List<Post>>>> GetAllPostsWithAccepted()
        {
            var response = await _postService.GetAllPostsWithAccepted();
            return Ok(response);
        }

        [HttpGet("admin/all"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<List<Post>>>> AdminGetPosts()
        {
            var response = await _postService.AdminGetPosts();
            return Ok(response);
        }

        [HttpGet("admin/{page}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<PostDTO>>> AdminGetPostsPaginated(int page)
        {
            var response = await _postService.AdminGetPostsPaginated(page);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<Post>>> GetPost(int id)
        {
            var result = await _postService.GetPost(id);
            return Ok(result);
        }

        [HttpGet("name-{studentId}")]
        public async Task<ActionResult<ServiceResponse<string>>> GetNameOfPoster(int studentId)
        {
            var result = await _postService.GetNameOfPoster(studentId);
            return Ok(result);
        }

        [HttpGet("fullName-{postId}")]
        public async Task<ActionResult<ServiceResponse<string>>> GetNameOfPosterByPostId(int postId)
        {
            var result = await _postService.GetNameOfPosterByPostId(postId);
            return Ok(result);
        }

        [HttpGet("interests-{postId}")]
        public async Task<ActionResult<ServiceResponse<List<PostInterest>>>> GetPostsInterests(int postId)
        {
            var result = await _postService.GetPostsInterests(postId);
            return Ok(result);
        }

        [HttpGet("postInterests")]
        public async Task<ActionResult<ServiceResponse<List<PostInterest>>>> GetAllPostInterests()
        {
            var result = await _postService.GetAllPostInterests();
            return Ok(result);
        }

        [HttpGet("admin/search/{searchText}/{page}")]
        public async Task<ActionResult<ServiceResponse<PostDTO>>> AdminSearchPosts(string searchText, int page)
        {
            var result = await _postService.AdminSearchPosts(searchText, page);
            return Ok(result);
        }

        [HttpGet("searchsuggestions/{searchText}")]
        public async Task<ActionResult<ServiceResponse<List<string>>>> GetPostSearchSuggestions(string searchText)
        {
            var result = await _postService.GetPostSearchSuggestions(searchText);
            return Ok(result);
        }

        [HttpGet("admin/searchsuggestions/{searchText}")]
        public async Task<ActionResult<ServiceResponse<List<string>>>> AdminGetPostSearchSuggestions(string searchText)
        {
            var result = await _postService.AdminGetPostSearchSuggestions(searchText);
            return Ok(result);
        }

        [HttpGet("studentsPostsPaginated/{studentId}/{page}")]
        public async Task<ActionResult<ServiceResponse<List<Post>>>> GetStudentsPostsPaginated(int studentId, int page)
        {
            var result = await _postService.GetStudentsPostsPaginated(studentId, page);
            return Ok(result);
        }

        [HttpGet("studentsPostsCount/{studentId}")]
        public async Task<ActionResult<ServiceResponse<int>>> GetStudentsPostsCount(int studentId)
        {
            var result = await _postService.GetStudentsPostsCount(studentId);
            return Ok(result);
        }

        [HttpGet("loggedInStudentsPostsPaginated/{page}")]
        public async Task<ActionResult<ServiceResponse<List<Post>>>> GetLoggedInStudentsPostsPaginated(int page)
        {
            var result = await _postService.GetLoggedInStudentsPostsPaginated(page);
            return Ok(result);
        }

        [HttpGet("loggedInStudentsPostsCount")]
        public async Task<ActionResult<ServiceResponse<int>>> GetLoggedInStudentsPostsCount()
        {
            var result = await _postService.GetLoggedInStudentsPostsCount();
            return Ok(result);
        }

        [HttpGet("admin/title/{postId}")]
        public async Task<ActionResult<ServiceResponse<string>>> AdminGetPostTitle(int postId)
        {
            var result = await _postService.AdminGetPostTitle(postId);
            return Ok(result);
        }

        [HttpGet("authors")]
        public async Task<ActionResult<ServiceResponse<Dictionary<int, string>>>> GetPostsAuthors()
        {
            var response = await _postService.GetPostsAuthors();
            return Ok(response);
        }

        [HttpGet("authorsProfilePic")]
        public async Task<ActionResult<ServiceResponse<Dictionary<int, string>>>> GetPostsAuthorsProfilePics()
        {
            var response = await _postService.GetPostsAuthorsProfilePics();
            return Ok(response);
        }

        [HttpGet("postsIsAccepted/{postId}")]
        public async Task<ActionResult<ServiceResponse<bool>>> GetPostsIsAccepted(int postId)
		{
            var response = await _postService.GetPostsIsAccepted(postId);
            return Ok(response);
		}
        #endregion

        #region "Post Methods"
        [Authorize(Roles = "Student")]
        [HttpPost]
        public async Task<ActionResult<ServiceResponse<Post>>> AddPost(Post post)
        {
            var response = await _postService.AddPost(post);
            return Ok(response);
        }

        [HttpPost("{page}")]
        public async Task<ActionResult<ServiceResponse<PostDTO>>> GetPostsPagination(int page, List<Post> posts)
        {
            var response = await _postService.GetPostsPagination(page, posts);
            return Ok(response);
        }

        [HttpPost("search/{searchText}/{page}")]
        public async Task<ActionResult<ServiceResponse<PostDTO>>> SearchPosts(string searchText, int page, List<Post> posts)
        {
            var result = await _postService.SearchPosts(searchText, page, posts);
            return Ok(result);
        }
        #endregion

        [HttpPut]
        public async Task<ActionResult<ServiceResponse<Tag>>> UpdatePost(Post post)
        {
            var result = await _postService.UpdatePost(post);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<Post>>> DeletePost(int id)
        {
            var result = await _postService.DeletePost(id);
            return Ok(result);
        }
    }
}
