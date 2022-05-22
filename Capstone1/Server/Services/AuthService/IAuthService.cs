namespace Capstone1.Server.Services.AuthService
{
    public interface IAuthService
    {

        #region "Auth Methods"
        #region "Auth Login Helper Methods"
        Task<bool> UserExists(string email);
        Task<bool> UserExists(string firstName, string lastName);
        #endregion

        #region "Post Methods"
        Task<ServiceResponse<int>> Register(User user, string password);
        Task<ServiceResponse<string>> Login(string email, string password);
        Task<ServiceResponse<bool>> ChangePassword(int userId, string newPassword);
        Task<ServiceResponse<bool>> SecurityPasswordChange(string currentPassword);
        #endregion
        #endregion

        #region "User Methods"
        #region "Get Methods"
        Task<ServiceResponse<int>> GetCurrentUserId();
        Task<int> GetUserId();
        Task<bool> CheckUserExpiration();
        Task<ServiceResponse<bool>> IsAdmin();
        Task<ServiceResponse<UserChange>> GetUserProfile();
        Task<ServiceResponse<UserChange>> GetUserProfileById(int userId);
        Task<ServiceResponse<UserDTO>> GetUsersPaginated(int page);
        Task<ServiceResponse<List<UserLanguage>>> GetAllUserLanguages();
        Task<ServiceResponse<UserDTO>> SearchUsers(string searchText, int page);
        Task<ServiceResponse<List<string>>> GetUserSearchSuggestions(string searchText);
        Task<ServiceResponse<UserDTO>> GetAdminUsersPaginated(int page);
        Task<ServiceResponse<UserDTO>> SearchAdminUsers(string searchText, int page);
        Task<ServiceResponse<List<string>>> GetAdminUserSearchSuggestions(string searchText);
        Task<ServiceResponse<int>> GetFirstAdminId();
        Task<ServiceResponse<string>> GetUserFullName(int userId);
        #endregion

        #region "Post Methods"
        Task<ServiceResponse<bool>> ChangeUserProfile(string firstName, string lastName, string ProfilePic, bool visible, List<UserLanguage> userLanguages);
        Task<ServiceResponse<bool>> ChangeUserProfileById(int id, string firstName, string lastName, string ProfilePic, bool visible, List<UserLanguage> userLanguages);
        #endregion

        #region "Delete Methods"
        Task<ServiceResponse<List<User>>> DeleteUser(int id, int page);
        #endregion
        #endregion

    }
}
