namespace Capstone1.Client.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _http;

        public event Action UsersChanged;
        #region "Lists"
        public List<User> Users { get; set; }
        public List<User> AdminUsers { get; set; }
        #endregion

        public int CurrentPage { get; set; } = 1;
        public int PageCount { get; set; } = 0;
        public string LastSearchText { get; set; } = string.Empty;

        public AuthService(HttpClient http)
        {
            _http = http;
        }

        #region "Auth Methods"
        #region "Post Methods"
        public async Task<ServiceResponse<bool>> ChangePassword(UserChangePassword request)
        {
            var result = await _http.PostAsJsonAsync("api/auth/change-password", request.Password);
            return await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
        }

        public async Task<ServiceResponse<string>> Login(UserLogin request)
        {
            var result = await _http.PostAsJsonAsync("api/auth/login", request);
            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<int>> Register(UserRegister request)
        {
            var result = await _http.PostAsJsonAsync("api/auth/register", request);
            return await result.Content.ReadFromJsonAsync<ServiceResponse<int>>();
        }

        public async Task<ServiceResponse<bool>> SecurityPasswordChange(UserPasswordSecure request)
        {
            var result = await _http.PostAsJsonAsync("api/auth/password-check", request.Password);
            return await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
        }
        #endregion
        #endregion

        #region "User Methods"
        #region "Get Methods"
        public async Task<UserChange> GetUserProfile()
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<UserChange>>("api/auth/profile");
            return result.Data;
        }

        public async Task<UserChange> GetUserProfileById(int userId)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<UserChange>>($"api/auth/admin/profile/{userId}");
            return result.Data;
        }

        public async Task<int> GetCurrentUserId()
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<int>>("api/auth/userId");
            return result.Data;
        }

        public async Task GetUsersPaginated(int page)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<UserDTO>>($"api/auth/paginated/{page}");
            if (result != null && result.Data != null)
            {
                Users = result.Data.Users;
                CurrentPage = result.Data.CurrentPage;
                PageCount = result.Data.Pages;
            }
        }

        public async Task GetAdminUsersPaginated(int page)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<UserDTO>>($"api/auth/admin/paginated/{page}");
            if (result != null && result.Data != null)
            {
                AdminUsers = result.Data.Users;
                CurrentPage = result.Data.CurrentPage;
                PageCount = result.Data.Pages;
            }
        }

        public async Task<List<UserLanguage>> GetAllUserLanguages()
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<UserLanguage>>>("api/auth/userLanguages");
            return result.Data;
        }

        public async Task SearchUsers(string searchText, int page)
        {
            LastSearchText = searchText;
            var result = await _http
                .GetFromJsonAsync<ServiceResponse<UserDTO>>($"api/auth/search/{searchText}/{page}");
            if (result != null && result.Data != null)
            {
                Users = result.Data.Users;
                CurrentPage = result.Data.CurrentPage;
                PageCount = result.Data.Pages;
            }

            UsersChanged?.Invoke();
        }

        public async Task SearchAdminUsers(string searchText, int page)
        {
            LastSearchText = searchText;
            var result = await _http
                .GetFromJsonAsync<ServiceResponse<UserDTO>>($"api/auth/admin/search/{searchText}/{page}");
            if (result != null && result.Data != null)
            {
                AdminUsers = result.Data.Users;
                CurrentPage = result.Data.CurrentPage;
                PageCount = result.Data.Pages;
            }

            UsersChanged.Invoke();
        }

        public async Task<List<string>> GetUserSearchSuggestions(string searchText)
        {
            var result = await _http
                .GetFromJsonAsync<ServiceResponse<List<string>>>($"api/auth/searchsuggestions/{searchText}");
            return result.Data;
        }

        public async Task<List<string>> GetAdminUserSearchSuggestions(string searchText)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<string>>>($"api/auth/admin/searchsuggestions/{searchText}");
            return result.Data;
        }

        public async Task<bool> IsAdmin()
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<bool>>("api/auth/admin/role-check");
            return result.Data;
        }

        public async Task<bool> CheckUserExpiration()
        {
            var result = await _http.GetFromJsonAsync<bool>("api/auth/expiration");
            return result;
        }

        public async Task<int> GetFirstAdminId()
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<int>>("api/auth/firstAdminId");
            return result.Data;
        }

        public async Task<string> GetUserFullName(int userId)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<string>>($"api/auth/userFullName/{userId}");
            return result.Data;
        }
        #endregion

        #region "Post Methods"
        public async Task<ServiceResponse<bool>> ChangeUserProfile(UserChange request)
        {
            var result = await _http.PostAsJsonAsync("api/auth/change-profile", request);
            return await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
        }

        public async Task<ServiceResponse<bool>> ChangeUserProfileById(int id, UserChange request)
        {
            var result = await _http.PostAsJsonAsync($"api/auth/admin/change-profile/{id}", request);
            return await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
        }
        #endregion

        #region "Delete Methods"
        public async Task DeleteUser(int id, int page)
        {
            var result = await _http.DeleteAsync($"api/auth/admin/{id}/{page}");
            AdminUsers = (await result.Content.ReadFromJsonAsync<ServiceResponse<List<User>>>()).Data;
            UsersChanged.Invoke();
        }
        #endregion
        #endregion
    }
}
