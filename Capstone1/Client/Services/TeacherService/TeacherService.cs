namespace Capstone1.Client.Services.TeacherService
{
    public class TeacherService : ITeacherService
    {
        private readonly HttpClient _http;

		public event Action TeachersChanged;

		public TeacherService(HttpClient http)
        {
            _http = http;
        }

        #region "Lists"
        public List<TeacherJob> LoggedInTeachersJobs { get; set; }
        public List<TeacherCourse> LoggedInTeachersCourses { get; set; }
        public List<User> Teachers { get; set; }
        #endregion

        #region "Get Methods"
        public async Task GetLoggedInTeachersCourses()
        {
            var result = await _http
                .GetFromJsonAsync<ServiceResponse<List<TeacherCourse>>>("api/teacher/current-teacher-courses");
            if (result != null && result.Data != null)
                LoggedInTeachersCourses = result.Data;
        }

        public async Task AdminGetTeachersCourses(int teacherId)
        {
            var result = await _http
                .GetFromJsonAsync<ServiceResponse<List<TeacherCourse>>>($"api/teacher/teacherCourses/{teacherId}");
            if (result != null && result.Data != null)
                LoggedInTeachersCourses = result.Data;
        }

        public async Task<List<TeacherInterest>> GetLoggedInTeachersInterests()
        {
            var result = await _http
                .GetFromJsonAsync<ServiceResponse<List<TeacherInterest>>>("api/teacher/current-teacher-interests");
            return result.Data;
        }

        public async Task GetLoggedInTeachersJobs()
        {
            var result = await _http
                .GetFromJsonAsync<ServiceResponse<List<TeacherJob>>>("api/teacher/current-teacher-jobs");
            if (result != null && result.Data != null)
                LoggedInTeachersJobs = result.Data;
        }

        public async Task AdminGetTeachersJobs(int teacherId)
        {
            var result = await _http
                .GetFromJsonAsync<ServiceResponse<List<TeacherJob>>>($"api/teacher/teacherJobs/{teacherId}");
            if (result != null && result.Data != null)
                LoggedInTeachersJobs = result.Data;
        }

        public async Task<List<TeacherCourseInterest>> GetAllTeacherCourseInterests()
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<TeacherCourseInterest>>>($"api/teacher/all-teacher-course-interests");
            if (result != null && result.Data != null)
                return result.Data;
            else
                return new List<TeacherCourseInterest>();
        }

        public async Task<bool> TeacherRoleCheck()
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<bool>>("api/teacher/role-check");
            return result.Data;
        }

        public async Task<ServiceResponse<User>> GetTeacher(int teacherId)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<User>>($"api/teacher/{teacherId}");
            return result;
        }

        public async Task<List<TeacherInterest>> GetTeachersInterests(int teacherId)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<TeacherInterest>>>($"api/teacher/interests/{teacherId}");
            return result.Data;
        }

        public async Task<List<TeacherJob>> GetTeachersJobs(int teacherId)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<TeacherJob>>>($"api/teacher/teacherJobs/{teacherId}");
            return result.Data;
        }

        public async Task<List<TeacherCourse>> GetTeachersCourses(int teacherId)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<TeacherCourse>>>($"api/teacher/teacherCourses/{teacherId}");
            return result.Data;
        }

        public async Task<List<UserLanguage>> GetTeachersLanguages(int teacherId)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<UserLanguage>>>($"api/teacher/languages/{teacherId}");
            return result.Data;
        }

        public async Task<List<TeacherInterest>> GetAllTeacherInterests()
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<TeacherInterest>>>("api/teacher/teacherInterests");
            return result.Data;
        }

        public async Task RandomTeachers(int num)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<User>>>($"api/teacher/random/{num}");
            if (result != null && result.Data != null)
                Teachers = result.Data;
        }

        public async Task<int> GetTeachersCount()
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<int>>("api/teacher/count");
            return result.Data;
        }

        public async Task GetTeachersPaginated(int page)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<User>>>($"api/teacher/paginated/{page}");
            if (result != null && result.Data != null)
                Teachers = result.Data;
        }

        public async Task SearchTeachers(string searchText, int page)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<User>>>($"api/teacher/search/{searchText}/{page}");
            if (result != null && result.Data != null)
                Teachers = result.Data;

            TeachersChanged.Invoke();
        }

        public async Task<List<string>> GetTeacherSearchSuggestions(string searchText)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<string>>>($"api/teacher/searchsuggestions/{searchText}");
            return result.Data;
        }

        public async Task<int> SearchTeachersCount(string searchText)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<int>>($"api/teacher/count/{searchText}");
            return result.Data;
        }
        #endregion

        #region "Post Methods"
        public async Task AddTeacherCourse(TeacherCourse teacherCourse)
        {
            var result = await _http.PostAsJsonAsync("api/teacher/teacher-course", teacherCourse);
            LoggedInTeachersCourses = (await result.Content
                .ReadFromJsonAsync<ServiceResponse<List<TeacherCourse>>>()).Data;
        }

        public async Task AddTeacherJob(TeacherJob teacherJob)
        {
            var result = await _http.PostAsJsonAsync("api/teacher/teacher-job", teacherJob);
            LoggedInTeachersJobs = (await result.Content
                .ReadFromJsonAsync<ServiceResponse<List<TeacherJob>>>()).Data;
        }
        #endregion

        #region "Put Methods"
        public async Task<ServiceResponse<bool>> ChangeInterests(Teacher teacher)
        {
            var result = await _http.PutAsJsonAsync("api/teacher", teacher);
            return await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
        }
        #endregion


        public TeacherCourse CreateNewTeacherCourse()
        {
            var newTeacherCourse = new TeacherCourse { IsNew = true, Editing = true };
            LoggedInTeachersCourses.Add(newTeacherCourse);
            return newTeacherCourse;
        }

        public TeacherJob CreateNewTeacherJob()
        {
            var newTeacherJob = new TeacherJob { IsNew = true, Editing = true };
            LoggedInTeachersJobs.Add(newTeacherJob);
            //OnChange.Invoke();
            return newTeacherJob;
        }

        #region "Delete Methods"
        public async Task DeleteTeacherCourse(int id)
        {
            var result = await _http.DeleteAsync($"api/teacher/teacher-course/{id}");
            LoggedInTeachersCourses = (await result.Content
                .ReadFromJsonAsync<ServiceResponse<List<TeacherCourse>>>()).Data;
        }

        public async Task DeleteTeacherJob(int id)
        {
            var result = await _http.DeleteAsync($"api/teacher/teacher-job/{id}");
            LoggedInTeachersJobs = (await result.Content
                .ReadFromJsonAsync<ServiceResponse<List<TeacherJob>>>()).Data;
        }
        #endregion

        #region "Put Methods"
        public async Task UpdateTeacherCourse(TeacherCourse teacherCourse)
        {
            var result = await _http.PutAsJsonAsync("api/teacher/teacher-course", teacherCourse);
            LoggedInTeachersCourses = (await result.Content
                .ReadFromJsonAsync<ServiceResponse<List<TeacherCourse>>>()).Data;
        }

        public async Task UpdateTeacherJob(TeacherJob teacherJob)
        {
            var result = await _http.PutAsJsonAsync("api/teacher/teacher-job", teacherJob);
            LoggedInTeachersJobs = (await result.Content
                .ReadFromJsonAsync<ServiceResponse<List<TeacherJob>>>()).Data;
        }
        #endregion
	}
}
