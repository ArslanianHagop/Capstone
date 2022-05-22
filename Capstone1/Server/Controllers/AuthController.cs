using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Capstone1.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        #region "Get Methods"

        [HttpGet("admin/role-check")]
        public async Task<ActionResult<ServiceResponse<bool>>> IsAdmin()
        {
            var response = await _authService.IsAdmin();
            return Ok(response);
        }

        [HttpGet("userId"), Authorize]
        public async Task<ActionResult<ServiceResponse<int>>> GetCurrentUserId()
        {
            var response = await _authService.GetCurrentUserId();
            return Ok(response);
        }

        [HttpGet("paginated/{page}")]
        public async Task<ActionResult<ServiceResponse<UserDTO>>> GetUsersPaginated(int page)
        {
            var response = await _authService.GetUsersPaginated(page);
            return Ok(response);
        }

        [HttpGet("admin/paginated/{page}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<UserDTO>>> GetAdminUsersPaginated(int page)
        {
            var response = await _authService.GetAdminUsersPaginated(page);
            return Ok(response);
        }

        [HttpGet("userLanguages")]
        public async Task<ActionResult<ServiceResponse<List<UserLanguage>>>> GetAllUserLanguages()
        {
            var response = await _authService.GetAllUserLanguages();
            return Ok(response);
        }

        [HttpGet("search/{searchText}/{page}")]
        public async Task<ActionResult<ServiceResponse<UserDTO>>> SearchUsers(string searchText, int page)
        {
            var response = await _authService.SearchUsers(searchText, page);
            return Ok(response);
        }

        [HttpGet("admin/search/{searchText}/{page}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<UserDTO>>> SearchAdminUsers(string searchText, int page)
        {
            var response = await _authService.SearchAdminUsers(searchText, page);
            return Ok(response);
        }

        [HttpGet("searchsuggestions/{searchText}")]
        public async Task<ActionResult<ServiceResponse<UserDTO>>> GetUserSearchSuggestions(string searchText)
        {
            var response = await _authService.GetUserSearchSuggestions(searchText);
            return Ok(response);
        }

        [HttpGet("admin/searchsuggestions/{searchText}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<UserDTO>>> GetAdminUserSearchSuggestions(string searchText)
        {
            var response = await _authService.GetAdminUserSearchSuggestions(searchText);
            return Ok(response);
        }

        [HttpGet("profile"), Authorize]
        public async Task<ActionResult<ServiceResponse<UserChange>>> GetUserProfile()
        {
            var response = await _authService.GetUserProfile();

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpGet("admin/profile/{userId}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<UserChange>>> GetUserProfileById(int userId)
        {
            var response = await _authService.GetUserProfileById(userId);
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpGet("expiration")]
        public async Task<ActionResult<ServiceResponse<bool>>> CheckUserExpiration()
        {
            var response = await _authService.CheckUserExpiration();
            return Ok(response);
        }

        [HttpGet("firstAdminId")]
        public async Task<ActionResult<ServiceResponse<int>>> GetFirstAdminId()
        {
            var response = await _authService.GetFirstAdminId();
            return Ok(response);
        }

        [HttpGet("userFullName/{userId}")]
        public async Task<ActionResult<ServiceResponse<string>>> GetUserFullName(int userId)
        {
            var response = await _authService.GetUserFullName(userId);
            return Ok(response);
        }
        #endregion

        #region "Post Methods"
        [HttpPost("register")]
        public async Task<ActionResult<ServiceResponse<int>>> Register(UserRegister request)
        {
            var response = await _authService.Register(new User
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                ProfilePic = request.ProfilePic,
                UserLanguages = request.UserLanguages,
                Role = request.Role,
            },
            request.Password);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<ActionResult<ServiceResponse<string>>> Login(UserLogin request)
        {
            var response = await _authService.Login(request.Email, request.Password);
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost("change-password"), Authorize]
        public async Task<ActionResult<ServiceResponse<bool>>> ChangePassword([FromBody] string newPassword)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);//FindFirstValue returns a string, so we parse it.
            var response = await _authService.ChangePassword(int.Parse(userId), newPassword);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost("change-profile"), Authorize]
        public async Task<ActionResult<ServiceResponse<bool>>> ChangeUserProfile(UserChange request)
        {
            var response = await _authService.ChangeUserProfile(request.FirstName, request.LastName, request.ProfilePic, request.Visible, request.UserLanguages);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost("admin/change-profile/{id}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<bool>>> ChangeUserProfileById(int id, UserChange request)
        {
            var response = await _authService.ChangeUserProfileById(id, request.FirstName, request.LastName, request.ProfilePic, request.Visible, request.UserLanguages);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost("password-check"), Authorize]
        public async Task<ActionResult<ServiceResponse<bool>>> SecurityPasswordChange([FromBody] string currentPassword)
        {
            var response = await _authService.SecurityPasswordChange(currentPassword);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
        #endregion


        [HttpDelete("admin/{id}/{page}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<List<User>>>> DeleteUser(int id, int page)
        {
            var response = await _authService.DeleteUser(id, page);
            return Ok(response);
        }
    }
}
