namespace Capstone1.Server.Services.StudentService
{
    public interface IStudentService
    {
        #region "Get Methods"
        Task<ServiceResponse<int>> GetStudentsCount();
        Task<ServiceResponse<List<User>>> GetStudentsPaginated(int page);
        Task<ServiceResponse<List<User>>> SearchStudents(string searchText, int page);
        Task<ServiceResponse<List<string>>> GetStudentSearchSuggestions(string searchText);
        Task<ServiceResponse<int>> SearchStudentsCount(string searchText);
        Task<ServiceResponse<User>> GetStudent(int studentId);
        Task<ServiceResponse<List<StudentInterest>>> GetLoggedInStudentsInterests();
        Task<ServiceResponse<bool>> StudentRoleCheck();
        Task<ServiceResponse<List<UserLanguage>>> GetStudentsLanguages(int studentId);
        Task<ServiceResponse<List<StudentInterest>>> GetStudentsInterests(int studentId);
        Task<ServiceResponse<List<StudentInterest>>> GetAllStudentInterests();
        #endregion
        #region "Put Methods"
        Task<ServiceResponse<bool>> ChangeInterests(Student student);
        #endregion
    }
}
