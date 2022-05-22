namespace Capstone1.Server.Services.StudentService
{
    public class StudentService : IStudentService
    {
        private readonly DataContext _context;
        private readonly IAuthService _authService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public StudentService(DataContext context, IAuthService authService, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _authService = authService;
            _httpContextAccessor = httpContextAccessor;
        }

        #region "Get Methods"
        public async Task<ServiceResponse<List<StudentInterest>>> GetAllStudentInterests()
        {
            List<StudentInterest> studentInterests = await _context.StudentInterests.ToListAsync();
            if (studentInterests == null || studentInterests.Count == 0)
            {
                return new ServiceResponse<List<StudentInterest>>
                {
                    Success = false,
                    Message = "No student has any interests."
                };
            }

            return new ServiceResponse<List<StudentInterest>> { Data = studentInterests };
        }

        public async Task<ServiceResponse<List<StudentInterest>>> GetLoggedInStudentsInterests()
        {
            return await GetStudentsInterestsById(await _authService.GetUserId());
        }

        public async Task<ServiceResponse<User>> GetStudent(int studentId)
        {
            var student = await _context.Users.FirstOrDefaultAsync(u => u.Id == studentId && !u.Deleted && (u.Visible || u.Role.Equals("Admin")) && u.Role.Equals("Student"));
            if (student == null)
            {
                return new ServiceResponse<User>
                {
                    Success = false,
                    Message = "This student was not found."
                };
            }

            return new ServiceResponse<User> { Data = student };
        }

        public async Task<ServiceResponse<List<StudentInterest>>> GetStudentsInterests(int studentId)
        {
            if (!(await GetStudent(studentId)).Success)
            {
                return new ServiceResponse<List<StudentInterest>>
                {
                    Success = false,
                    Message = "This student doesn't exist to get student's interests."
                };
            }

            return await GetStudentsInterestsById(studentId);
        }

        private async Task<ServiceResponse<List<StudentInterest>>> GetStudentsInterestsById(int studentId)
        {
            List<StudentInterest> studentsInterests = await _context.StudentInterests.Where(si => si.StudentId == studentId).ToListAsync();
            if (studentsInterests == null || studentsInterests.Count == 0)
            {
                return new ServiceResponse<List<StudentInterest>>
                {
                    Data = new List<StudentInterest>(),
                    Success = false,
                    Message = "This student has no interests."
                };
            }

            return new ServiceResponse<List<StudentInterest>> { Data = studentsInterests };
        }

        public async Task<ServiceResponse<List<UserLanguage>>> GetStudentsLanguages(int studentId)
        {
            if (!(await GetStudent(studentId)).Success)
            {
                return new ServiceResponse<List<UserLanguage>>
                {
                    Success = false,
                    Message = "This student doesn't exist to get student's languages."
                };
            }
            List<UserLanguage> studentsLanguages = await _context.UserLanguages.Where(ul => ul.UserId == studentId).ToListAsync();
            if (studentsLanguages == null || studentsLanguages.Count == 0)
            {
                return new ServiceResponse<List<UserLanguage>>
                {
                    Success = false,
                    Message = "This student knows no languages."
                };
            }

            return new ServiceResponse<List<UserLanguage>> { Data = studentsLanguages };
        }

        public async Task<ServiceResponse<bool>> StudentRoleCheck()
        {
            if (!_httpContextAccessor.HttpContext.User.IsInRole("Student"))
            {
                return new ServiceResponse<bool>
                {
                    Success = false,
                    Message = "You're not a student."
                };
            }

            return new ServiceResponse<bool> { Data = true, Message = "You are a student." };
        }

        private async Task<ServiceResponse<List<User>>> GetStudents()
        {
            List<User> students = new List<User>();
            int? currentUserId = await _authService.GetUserId();
            if(currentUserId == 0)
                students = await _context.Users.Where(u => !u.Deleted && u.Visible && u.Role.Equals("Student")).ToListAsync();
            else
                students = await _context.Users.Where(u => !u.Deleted && u.Visible && u.Role.Equals("Student") && u.Id != currentUserId).ToListAsync();
            if (students == null || students.Count == 0)
            {
                return new ServiceResponse<List<User>>
                {
                    Success = false,
                    Message = "No students were found."
                };
            }

            return new ServiceResponse<List<User>> { Data = students };
        }

        public async Task<ServiceResponse<int>> GetStudentsCount()
        {
            var studentsCount = await _context.Users.Where(u => !u.Deleted && u.Visible && u.Role.Equals("Student")).CountAsync();
            return new ServiceResponse<int> { Data = studentsCount };
        }

        public async Task<ServiceResponse<List<User>>> GetStudentsPaginated(int page)
        {
            var pageResults = 5f;
            var studentsPaginated = (await GetStudents()).Data.Skip((page - 1) * (int)pageResults).Take((int)pageResults).ToList();
            if (studentsPaginated == null || studentsPaginated.Count == 0)
            {
                return new ServiceResponse<List<User>>
                {
                    Success = false,
                    Message = "No students found."
                };
            }

            return new ServiceResponse<List<User>> { Data = studentsPaginated };
        }

        public async Task<ServiceResponse<List<User>>> SearchStudents(string searchText, int page)
        {
            var pageResults = 5f;
            var students = await FindStudentsBySearchText(searchText);
            var studentsPaginated = students.Skip((page - 1) * (int)pageResults).Take((int)pageResults).ToList();

            return new ServiceResponse<List<User>> { Data = studentsPaginated };
        }

        private async Task<List<User>> FindStudentsBySearchText(string searchText)
        {
            int? currentUserId = await _authService.GetUserId();
            List<User> students = new List<User>();
            if(currentUserId == 0)
            {
                students = await _context.Users
                    .Where(u => (!u.Deleted && u.Visible && u.Role.Equals("Student"))
                    && (u.FirstName.ToLower().Contains(searchText.ToLower())
                    || u.LastName.ToLower().Contains(searchText.ToLower())
                    || (u.FirstName + " " + u.LastName).ToLower().Contains(searchText.ToLower()))).ToListAsync();
            }
            else
            {
                students = await _context.Users
                    .Where(u => (!u.Deleted && u.Visible && u.Role.Equals("Student") && u.Id != currentUserId)
                    && (u.FirstName.ToLower().Contains(searchText.ToLower())
                    || u.LastName.ToLower().Contains(searchText.ToLower())
                    || (u.FirstName + " " + u.LastName).ToLower().Contains(searchText.ToLower()))).ToListAsync();
            }

            return students;
        }

        public async Task<ServiceResponse<List<string>>> GetStudentSearchSuggestions(string searchText)
        {
            var students = await FindStudentsBySearchText(searchText);

            List<string> result = new List<string>();

            foreach (var student in students)
            {
                result.Add(student.FirstName + " " + student.LastName);
            }

            return new ServiceResponse<List<string>> { Data = result };
        }

        public async Task<ServiceResponse<int>> SearchStudentsCount(string searchText)
        {
            return new ServiceResponse<int> { Data = (await FindStudentsBySearchText(searchText)).Count };
        }
        #endregion

        #region "Put Methods"
        public async Task<ServiceResponse<bool>> ChangeInterests(Student student)
        {
            if (student.StudentId == 0)
            {
                student.StudentId = await _authService.GetUserId();
            }
            var dbStudent = await _context.Students.FindAsync(student.StudentId);
            if (dbStudent == null)
            {
                return new ServiceResponse<bool>
                {
                    Success = false,
                    Message = "Student not found."
                };
            }

			for (int i = 0; i < student.StudentInterests.Count; i++)
			{
				for (int j = i + 1; j < student.StudentInterests.Count; j++)
				{
                    if(student.StudentInterests[i].InterestId == student.StudentInterests[j].InterestId)
					{
                        return new ServiceResponse<bool>
                        {
                            Data = false,
                            Success = false,
                            Message = "You have selected the same interest more than once."
                        };
					}
				}
			}

            var dbStudentInterests = await _context.StudentInterests.Where(si => si.StudentId == student.StudentId).ToListAsync();
            foreach (var dbStudentInterest in dbStudentInterests)
            {
                _context.StudentInterests.Remove(dbStudentInterest);
            }

            await _context.SaveChangesAsync();

            foreach (var studentInterest in student.StudentInterests)
            {
                studentInterest.StudentId = student.StudentId;
            }

            dbStudent.StudentInterests = student.StudentInterests;
            await _context.SaveChangesAsync();

            return new ServiceResponse<bool> { Data = true, Message = "Interests Updated." };
        }
        #endregion
	}
}
