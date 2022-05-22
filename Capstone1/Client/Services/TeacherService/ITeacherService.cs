namespace Capstone1.Client.Services.TeacherService
{
    public interface ITeacherService
    {
        event Action TeachersChanged;
        #region "Get Methods"
        Task<List<TeacherInterest>> GetTeachersInterests(int teacherId);
        Task<List<TeacherInterest>> GetAllTeacherInterests();
        Task<List<UserLanguage>> GetTeachersLanguages(int teacherId);
        Task<List<TeacherJob>> GetTeachersJobs(int teacherId);
        Task AdminGetTeachersJobs(int teacherId);
        Task<List<TeacherCourse>> GetTeachersCourses(int teacherId);
        Task AdminGetTeachersCourses(int teacherId);
        Task<ServiceResponse<User>> GetTeacher(int teacherId);
        Task<bool> TeacherRoleCheck();
        Task<List<TeacherInterest>> GetLoggedInTeachersInterests();
        Task GetLoggedInTeachersJobs();
        Task GetLoggedInTeachersCourses();
        Task<List<TeacherCourseInterest>> GetAllTeacherCourseInterests();
        Task RandomTeachers(int num);
        Task<int> GetTeachersCount();
        Task GetTeachersPaginated(int page);
        Task SearchTeachers(string searchText, int page);
        Task<List<string>> GetTeacherSearchSuggestions(string searchText);
        Task<int> SearchTeachersCount(string searchText);
        #endregion
        #region "Lists"
        List<TeacherJob> LoggedInTeachersJobs { get; set; }
        List<TeacherCourse> LoggedInTeachersCourses { get; set; }
        List<User> Teachers { get; set; }
        #endregion
        #region "Put Methods"
        Task<ServiceResponse<bool>> ChangeInterests(Teacher teacher);
        Task UpdateTeacherJob(TeacherJob teacherJob);
        Task UpdateTeacherCourse(TeacherCourse teacherCourse);
        #endregion
        #region "Post Methods"
        Task AddTeacherJob(TeacherJob teacherJob);
        Task AddTeacherCourse(TeacherCourse teacherCourse);
        #endregion
        #region "Delete Methods"
        Task DeleteTeacherJob(int id);
        Task DeleteTeacherCourse(int id);
        #endregion
        TeacherJob CreateNewTeacherJob();
        TeacherCourse CreateNewTeacherCourse();
    }
}
