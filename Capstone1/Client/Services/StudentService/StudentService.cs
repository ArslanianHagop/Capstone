namespace Capstone1.Client.Services.StudentService
{
    public class StudentService : IStudentService
    {
        private readonly HttpClient _http;

        public StudentService(HttpClient http)
        {
            _http = http;
        }

        #region "Lists"
        public List<User> Students { get; set; }
        #endregion


		public event Action StudentsChanged;

        #region "Put Methods"
        public async Task<ServiceResponse<bool>> ChangeInterests(Student student)
        {
            var result = await _http.PutAsJsonAsync("api/student", student);
            return await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
        }
        #endregion

        #region "Get Methods"
        public async Task<List<StudentInterest>> GetAllStudentInterests()
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<StudentInterest>>>("api/student/studentInterests");
            return result.Data;
        }

        public async Task<List<StudentInterest>> GetLoggedInStudentsInterests()
        {
            var result = await _http
                .GetFromJsonAsync<ServiceResponse<List<StudentInterest>>>("api/student/current-student-interests");
            return result.Data;
        }

		public async Task<ServiceResponse<User>> GetStudent(int studentId)
		{
            var result = await _http.GetFromJsonAsync<ServiceResponse<User>>($"api/student/{studentId}");
            return result;
		}

		public async Task<int> GetStudentsCount()
		{
            var result = await _http.GetFromJsonAsync<ServiceResponse<int>>("api/student/count");
            return result.Data;
		}

		public async Task<List<string>> GetStudentSearchSuggestions(string searchText)
		{
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<string>>>($"api/student/searchsuggestions/{searchText}");
            return result.Data;
		}

		public async Task<List<StudentInterest>> GetStudentsInterests(int studentId)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<StudentInterest>>>($"api/student/interests/{studentId}");
            return result.Data;
        }

        public async Task<List<UserLanguage>> GetStudentsLanguages(int studentId)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<UserLanguage>>>($"api/student/languages/{studentId}");
            return result.Data;
        }

		public async Task GetStudentsPaginated(int page)
		{
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<User>>>($"api/student/paginated/{page}");
            if (result != null && result.Data != null)
                Students = result.Data;
        }

		public async Task SearchStudents(string searchText, int page)
		{
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<User>>>($"api/student/search/{searchText}/{page}");
            if (result != null && result.Data != null)
                Students = result.Data;

            StudentsChanged.Invoke();
		}

		public async Task<int> SearchStudentsCount(string searchText)
		{
            var result = await _http.GetFromJsonAsync<ServiceResponse<int>>($"api/student/count/{searchText}");
            return result.Data;
		}

		public async Task<bool> StudentRoleCheck()
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<bool>>("api/student/role-check");
            return result.Data;
        }
        #endregion
    }
}
