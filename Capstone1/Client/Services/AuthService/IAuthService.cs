namespace Capstone1.Client.Services.AuthService
{
    public interface IAuthService
    {
        event Action UsersChanged;
        #region "Lists"
        List<User> Users { get; set; }
        List<User> AdminUsers { get; set; }
        #endregion
        int CurrentPage { get; set; }
        int PageCount { get; set; }
        string LastSearchText { get; set; }
        #region "Auth Methods"
        Task<ServiceResponse<int>> Register(UserRegister request);
        Task<ServiceResponse<string>> Login(UserLogin request);
        Task<ServiceResponse<bool>> ChangePassword(UserChangePassword request);
        Task<ServiceResponse<bool>> SecurityPasswordChange(UserPasswordSecure request);
        Task<bool> CheckUserExpiration();
        #endregion

        #region "User Methods"
        #region "Get Methods"
        Task<int> GetCurrentUserId();
        Task<UserChange> GetUserProfile();
        Task<UserChange> GetUserProfileById(int userId);
        Task GetUsersPaginated(int page);
        Task<List<UserLanguage>> GetAllUserLanguages();
        Task SearchUsers(string searchText, int page);
        Task<List<string>> GetUserSearchSuggestions(string searchText);
        Task GetAdminUsersPaginated(int page);
        Task SearchAdminUsers(string searchText, int page);
        Task<List<string>> GetAdminUserSearchSuggestions(string searchText);
        Task<bool> IsAdmin();
        Task<int> GetFirstAdminId();
        Task<string> GetUserFullName(int userId);
        #endregion
        #region "Post Methods"
        Task<ServiceResponse<bool>> ChangeUserProfile(UserChange request);
        Task<ServiceResponse<bool>> ChangeUserProfileById(int id, UserChange request);
        #endregion

        #region "Delete Methods"
        Task DeleteUser(int id, int page);
        #endregion
        #endregion
    }
}
