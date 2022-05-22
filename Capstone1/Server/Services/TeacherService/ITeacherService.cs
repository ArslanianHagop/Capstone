namespace Capstone1.Server.Services.TeacherService
{
    public interface ITeacherService
    {
        #region "Get Methods"
        Task<ServiceResponse<List<User>>> GetTeachersPaginated(int page);
        Task<ServiceResponse<int>> GetTeachersCount();
        Task<ServiceResponse<List<User>>> SearchTeachers(string searchText, int page);
        Task<ServiceResponse<int>> SearchTeachersCount(string searchText);
        Task<ServiceResponse<List<string>>> GetTeacherSearchSuggestions(string searchText);
        Task<ServiceResponse<User>> GetTeacher(int teacherId);
        Task<ServiceResponse<List<UserLanguage>>> GetTeachersLanguages(int teacherId);
        Task<ServiceResponse<List<TeacherInterest>>> GetTeachersInterests(int teacherId);
        Task<ServiceResponse<List<TeacherInterest>>> GetLoggedInTeachersInterests();
        Task<ServiceResponse<bool>> TeacherRoleCheck();
        Task<ServiceResponse<List<TeacherJob>>> GetLoggedInTeachersJobs();
        Task<ServiceResponse<List<TeacherJob>>> GetTeachersJobs(int teacherId);
        Task<ServiceResponse<List<TeacherCourseInterest>>> GetAllTeacherCourseInterests();
        Task<ServiceResponse<List<TeacherInterest>>> GetAllTeacherInterests();
        Task<ServiceResponse<List<User>>> RandomTeachers(int num);
        Task<ServiceResponse<List<TeacherCourse>>> GetLoggedInTeachersCourses();
        Task<ServiceResponse<List<TeacherCourse>>> GetTeachersCourses(int teacherId);
        #endregion
        #region "Post Methods"
        Task<ServiceResponse<List<TeacherJob>>> AddTeacherJob(TeacherJob teacherJob);
        Task<ServiceResponse<List<TeacherCourse>>> AddTeacherCourse(TeacherCourse teacherCourse);
        #endregion
        #region "Put Methods"
        Task<ServiceResponse<bool>> ChangeInterests(Teacher teacher);
        Task<ServiceResponse<List<TeacherJob>>> UpdateTeacherJob(TeacherJob teacherJob);
        Task<ServiceResponse<List<TeacherCourse>>> UpdateTeacherCourse(TeacherCourse teacherCourse);
        #endregion
        #region "Delete Methods"
        Task<ServiceResponse<List<TeacherJob>>> DeleteTeacherJob(int id);
        Task<ServiceResponse<List<TeacherCourse>>> DeleteTeacherCourse(int id);
        #endregion
    }
}
