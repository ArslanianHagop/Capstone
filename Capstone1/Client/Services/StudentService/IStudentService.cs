namespace Capstone1.Client.Services.StudentService
{
    public interface IStudentService
    {
        event Action StudentsChanged;
        #region "Lists"
        List<User> Students { get; set; }
        #endregion
        #region "Get Methods"
        Task<ServiceResponse<User>> GetStudent(int studentId);
        Task<bool> StudentRoleCheck();
        Task<List<StudentInterest>> GetLoggedInStudentsInterests();
        Task<List<UserLanguage>> GetStudentsLanguages(int studentId);
        Task<List<StudentInterest>> GetStudentsInterests(int studentId);
        Task<List<StudentInterest>> GetAllStudentInterests();
        Task<int> GetStudentsCount();
        Task GetStudentsPaginated(int page);
        Task SearchStudents(string searchText, int page);
        Task<List<string>> GetStudentSearchSuggestions(string searchText);
        Task<int> SearchStudentsCount(string searchText);
        #endregion

        #region "Put Methods"
        Task<ServiceResponse<bool>> ChangeInterests(Student student);
        #endregion
    }
}
