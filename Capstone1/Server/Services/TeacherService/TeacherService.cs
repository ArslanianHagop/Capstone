namespace Capstone1.Server.Services.TeacherService
{
    public class TeacherService : ITeacherService
    {
        private readonly DataContext _context;
        private readonly IAuthService _authService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TeacherService(DataContext context, IAuthService authService, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _authService = authService;
            _httpContextAccessor = httpContextAccessor;
        }

        #region "Get Methods"
        public async Task<ServiceResponse<List<TeacherCourse>>> GetTeachersCourses(int teacherId)
        {
            if (!(await GetTeacher(teacherId)).Success)
            {
                return new ServiceResponse<List<TeacherCourse>>
                {
                    Success = false,
                    Message = "This teacher doesn't exist to get teacher's courses."
                };
            }
            return await GetTeachersCoursesById(teacherId);
        }

        public async Task<ServiceResponse<List<TeacherCourse>>> GetLoggedInTeachersCourses()
        {
            return await GetTeachersCoursesById(await _authService.GetUserId());
        }

        private async Task<ServiceResponse<List<TeacherCourse>>> GetTeachersCoursesById(int id)
        {
            List<TeacherCourse> teacherCourses = await _context.TeacherCourses.Where(tc => tc.TeacherId == id).ToListAsync();
            if (teacherCourses == null)
            {
                return new ServiceResponse<List<TeacherCourse>>
                {
                    Success = false,
                    Message = "This teacher has no courses."
                };
            }

            return new ServiceResponse<List<TeacherCourse>> { Data = teacherCourses };
        }

        public async Task<ServiceResponse<List<TeacherInterest>>> GetTeachersInterests(int teacherId)
        {
            if (!(await GetTeacher(teacherId)).Success)
            {
                return new ServiceResponse<List<TeacherInterest>>
                {
                    Success = false,
                    Message = "This teacher doesn't exist to get teacher's interests."
                };
            }
            return await GetTeachersInterestsById(teacherId);
        }

        public async Task<ServiceResponse<List<TeacherInterest>>> GetLoggedInTeachersInterests()
        {
            return await GetTeachersInterestsById(await _authService.GetUserId());
        }

        private async Task<ServiceResponse<List<TeacherInterest>>> GetTeachersInterestsById(int id)
        {
            List<TeacherInterest> teacherInterests = await _context.TeacherInterests.Where(ti => ti.TeacherId == id).ToListAsync();
            if (teacherInterests == null)
            {
                return new ServiceResponse<List<TeacherInterest>>
                {
                    Success = false,
                    Message = "This teacher has no interests."
                };
            }

            return new ServiceResponse<List<TeacherInterest>> { Data = teacherInterests };
        }

        public async Task<ServiceResponse<List<TeacherJob>>> GetTeachersJobs(int teacherId)
        {
            if (!(await GetTeacher(teacherId)).Success)
            {
                return new ServiceResponse<List<TeacherJob>>
                {
                    Success = false,
                    Message = "This teacher doesn't exist to get teacher's background."
                };
            }
            return await GetTeachersJobsById(teacherId);
        }

        public async Task<ServiceResponse<List<TeacherJob>>> GetLoggedInTeachersJobs()
        {
            return await GetTeachersJobsById(await _authService.GetUserId());
        }

        private async Task<ServiceResponse<List<TeacherJob>>> GetTeachersJobsById(int id)
        {
            List<TeacherJob> teacherJobs = await _context.TeacherJobs.Where(tj => tj.TeacherId == id).ToListAsync();
            if (teacherJobs == null)
            {
                return new ServiceResponse<List<TeacherJob>>
                {
                    Success = false,
                    Message = "This teacher has no jobs."
                };
            }

            return new ServiceResponse<List<TeacherJob>> { Data = teacherJobs };
        }

        public async Task<ServiceResponse<List<TeacherCourseInterest>>> GetAllTeacherCourseInterests()
        {
            var teacherCourseInterests = await _context.TeacherCourseInterests.ToListAsync();
            if (teacherCourseInterests == null)
            {
                return new ServiceResponse<List<TeacherCourseInterest>>
                {
                    Success = false,
                    Message = "There are no Teacher Course Interests"
                };
            }

            return new ServiceResponse<List<TeacherCourseInterest>> { Data = teacherCourseInterests };
        }

        public async Task<ServiceResponse<bool>> TeacherRoleCheck()
        {
            if (!_httpContextAccessor.HttpContext.User.IsInRole("Teacher"))
            {
                return new ServiceResponse<bool>
                {
                    Success = false,
                    Message = "You're not a teacher."
                };
            }

            return new ServiceResponse<bool> { Data = true, Message = "You are a teacher." };
        }

        public async Task<ServiceResponse<User>> GetTeacher(int teacherId)
        {
            var teacher = await _context.Users.FirstOrDefaultAsync(u => u.Id == teacherId && !u.Deleted && (u.Visible || u.Role.Equals("Admin")) && u.Role.Equals("Teacher"));
            if (teacher == null)
            {
                return new ServiceResponse<User>
                {
                    Success = false,
                    Message = "This teacher was not found."
                };
            }

            return new ServiceResponse<User> { Data = teacher };
        }

        public async Task<ServiceResponse<List<UserLanguage>>> GetTeachersLanguages(int teacherId)
        {
            if (!(await GetTeacher(teacherId)).Success)
            {
                return new ServiceResponse<List<UserLanguage>>
                {
                    Success = false,
                    Message = "This teacher doesn't exist to get teacher's languages."
                };
            }
            List<UserLanguage> teachersLanguages = await _context.UserLanguages.Where(ul => ul.UserId == teacherId).ToListAsync();
            if (teachersLanguages == null || teachersLanguages.Count == 0)
            {
                return new ServiceResponse<List<UserLanguage>>
                {
                    Success = false,
                    Message = "This teacher known no languages."
                };
            }

            return new ServiceResponse<List<UserLanguage>> { Data = teachersLanguages };
        }

        public async Task<ServiceResponse<List<TeacherInterest>>> GetAllTeacherInterests()
        {
            List<TeacherInterest> teacherInterests = await _context.TeacherInterests.ToListAsync();
            if (teacherInterests == null || teacherInterests.Count == 0)
            {
                return new ServiceResponse<List<TeacherInterest>>
                {
                    Success = false,
                    Message = "No teacher has any interests."
                };
            }

            return new ServiceResponse<List<TeacherInterest>> { Data = teacherInterests };
        }

        public async Task<ServiceResponse<List<User>>> RandomTeachers(int num)
        {
            Random random = new Random();
            List<User> teachers = await _context.Users.Where(u => !u.Deleted && u.Visible && u.Role.Equals("Teacher")).ToListAsync();
            List<User> result = new List<User>();
            if (teachers != null && teachers.Count != 0)
            {
                while(result.Count != (teachers.Count < num ? teachers.Count : num))
                {
                    int randomNumber = random.Next(teachers.Count);
                    if (!result.Contains(teachers[randomNumber]))
                        result.Add(teachers[randomNumber]);
                }
            }

            if (result == null || result.Count == 0)
            {
                return new ServiceResponse<List<User>>
                {
                    Success = false,
                    Message = $"No teachers were found to pick {num} randomly."
                };
            }

            return new ServiceResponse<List<User>> { Data = result };
        }

        public async Task<ServiceResponse<List<User>>> GetTeachersPaginated(int page)
        {
            var pageResults = 5f;
            var teachersPaginated = (await GetTeachers()).Data.Skip((page - 1) * (int)pageResults).Take((int)pageResults).ToList();
            if (teachersPaginated == null || teachersPaginated.Count == 0)
            {
                return new ServiceResponse<List<User>>
                {
                    Success = false,
                    Message = "No teachers found."
                };
            }

            return new ServiceResponse<List<User>> { Data = teachersPaginated };
        }

        public async Task<ServiceResponse<int>> GetTeachersCount()
        {
            var teachersCount = await _context.Users.Where(u => !u.Deleted && u.Visible && u.Role.Equals("Teacher")).CountAsync();
            return new ServiceResponse<int> { Data = teachersCount };
        }

        public async Task<ServiceResponse<List<User>>> SearchTeachers(string searchText, int page)
        {
            var pageResults = 5f;
            var teachers = await FindTeachersBySearchText(searchText);
            var teachersPaginated = teachers.Skip((page - 1) * (int)pageResults).Take((int)pageResults).ToList();

            return new ServiceResponse<List<User>> { Data = teachersPaginated };
        }

        public async Task<ServiceResponse<List<string>>> GetTeacherSearchSuggestions(string searchText)
        {
            var teachers = await FindTeachersBySearchText(searchText);

            List<string> result = new List<string>();

            foreach (var teacher in teachers)
            {
                result.Add(teacher.FirstName + " " + teacher.LastName);
            }

            return new ServiceResponse<List<string>> { Data = result };
        }

        public async Task<ServiceResponse<int>> SearchTeachersCount(string searchText)
        {
            return new ServiceResponse<int> { Data = (await FindTeachersBySearchText(searchText)).Count };
        }
        #endregion

        #region "Post Methods"
        public async Task<ServiceResponse<List<TeacherCourse>>> AddTeacherCourse(TeacherCourse teacherCourse)
        {
            teacherCourse.Editing = teacherCourse.IsNew = false;
            teacherCourse.TeacherId = await _authService.GetUserId();
            _context.TeacherCourses.Add(teacherCourse);
            await _context.SaveChangesAsync();
            return await GetLoggedInTeachersCourses();
        }

        public async Task<ServiceResponse<List<TeacherJob>>> AddTeacherJob(TeacherJob teacherJob)
        {
            teacherJob.Editing = teacherJob.IsNew = false;
            teacherJob.TeacherId = await _authService.GetUserId(); //maybe not needed
            _context.TeacherJobs.Add(teacherJob);
            await _context.SaveChangesAsync();
            return await GetLoggedInTeachersJobs();
        }
        #endregion

        #region "Put Methods"
        public async Task<ServiceResponse<bool>> ChangeInterests(Teacher teacher)
        {
            var currentUserId = await _authService.GetUserId();
            if (teacher.TeacherId == 0)
            {
                teacher.TeacherId = await _authService.GetUserId();
            }
            var dbTeacher = await _context.Teachers.FindAsync(teacher.TeacherId);
            if (dbTeacher == null)
            {
                return new ServiceResponse<bool>
                {
                    Success = false,
                    Message = "Teacher not found."
                };
            }

			for (int i = 0; i < teacher.TeacherInterests.Count; i++)
			{
				for (int j = i + 1; j < teacher.TeacherInterests.Count; j++)
				{
                    if(teacher.TeacherInterests[i].InterestId == teacher.TeacherInterests[j].InterestId)
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

            var dbTeacherInterests = await _context.TeacherInterests.Where(ti => ti.TeacherId == currentUserId).ToListAsync();
            foreach (var dbTeacherInterest in dbTeacherInterests)
            {
                _context.TeacherInterests.Remove(dbTeacherInterest);
            }

            await _context.SaveChangesAsync();

            foreach (var TeacherInterest in teacher.TeacherInterests)
            {
                TeacherInterest.TeacherId = teacher.TeacherId;
            }

            dbTeacher.TeacherInterests = teacher.TeacherInterests;
            await _context.SaveChangesAsync();


            return new ServiceResponse<bool> { Data = true, Message = "Interests Updated" };
        }

        public async Task<ServiceResponse<List<TeacherCourse>>> UpdateTeacherCourse(TeacherCourse teacherCourse)
        {
            var dbTeacherCourse = await _context.TeacherCourses.FindAsync(teacherCourse.Id);
            if (dbTeacherCourse == null)
            {
                return new ServiceResponse<List<TeacherCourse>>
                {
                    Success = false,
                    Message = "Teacher Course not found."
                };
            }

            dbTeacherCourse.Name = teacherCourse.Name;
            dbTeacherCourse.CourseLink = teacherCourse.CourseLink;

            var dbTeacherCourseInterests = await _context.TeacherCourseInterests.Where(tci => tci.TeacherCourseId == teacherCourse.Id).ToListAsync();
            foreach (var dbTeacherCourseInterest in dbTeacherCourseInterests)
            {
                _context.TeacherCourseInterests.Remove(dbTeacherCourseInterest);
            }

            await _context.SaveChangesAsync();

            foreach (var TeacherCourseInterest in teacherCourse.TeacherCourseInterests)
            {
                TeacherCourseInterest.TeacherCourseId = teacherCourse.TeacherId;
            }

            dbTeacherCourse.TeacherCourseInterests = teacherCourse.TeacherCourseInterests;
            await _context.SaveChangesAsync();

            return await GetTeachersCourses(teacherCourse.TeacherId);
        }

        public async Task<ServiceResponse<List<TeacherJob>>> UpdateTeacherJob(TeacherJob teacherJob)
        {
            var dbTeacherJob = await _context.TeacherJobs.FindAsync(teacherJob.Id);
            if (dbTeacherJob == null)
            {
                return new ServiceResponse<List<TeacherJob>>
                {
                    Success = false,
                    Message = "Teacher Job not found."
                };
            }

            dbTeacherJob.Position = teacherJob.Position;
            dbTeacherJob.CompanyName = teacherJob.CompanyName;
            dbTeacherJob.FromMonth = teacherJob.FromMonth;
            dbTeacherJob.FromYear = teacherJob.FromYear;
            dbTeacherJob.ToMonth = teacherJob.ToMonth;
            dbTeacherJob.ToYear = teacherJob.ToYear;

            await _context.SaveChangesAsync();

            return await GetTeachersJobs(teacherJob.TeacherId);
        }
        #endregion

        #region "Delete Methods"
        public async Task<ServiceResponse<List<TeacherCourse>>> DeleteTeacherCourse(int id)
        {
            TeacherCourse teacherCourse = await _context.TeacherCourses.FindAsync(id);
            if (teacherCourse == null)
            {
                return new ServiceResponse<List<TeacherCourse>>
                {
                    Success = false,
                    Message = "Teacher Course not found."
                };
            }

            teacherCourse.Deleted = true;
            await _context.SaveChangesAsync();

            return await GetLoggedInTeachersCourses();
        }

        public async Task<ServiceResponse<List<TeacherJob>>> DeleteTeacherJob(int id)
        {
            TeacherJob teacherJob = await _context.TeacherJobs.FindAsync(id);
            if (teacherJob == null)
            {
                return new ServiceResponse<List<TeacherJob>>
                {
                    Success = false,
                    Message = "Teacher Job not found."
                };
            }

            teacherJob.Deleted = true;
            await _context.SaveChangesAsync();

            return await GetLoggedInTeachersJobs();
        }
        #endregion

        #region "Private Methods"
        private async Task<ServiceResponse<List<User>>> GetTeachers()
        {
            List<User> teachers = new List<User>();
            int? currentUserId = await _authService.GetUserId();
            if(currentUserId == 0)
                teachers = await _context.Users.Where(u => !u.Deleted && u.Visible && u.Role.Equals("Teacher")).ToListAsync();
            else
                teachers = await _context.Users.Where(u => !u.Deleted && u.Visible && u.Role.Equals("Teacher") && u.Id != currentUserId).ToListAsync();
            if (teachers == null || teachers.Count == 0)
            {
                return new ServiceResponse<List<User>>
                {
                    Success = false,
                    Message = "No teachers were found."
                };
            }

            return new ServiceResponse<List<User>> { Data = teachers };
        }

        private async Task<List<User>> FindTeachersBySearchText(string searchText)
        {
            int? currentUserId = await _authService.GetUserId();
            List<User> teachers = new List<User>(); 
            if (currentUserId == 0)
            {
                teachers = await _context.Users
                    .Where(u => (!u.Deleted && u.Visible && u.Role.Equals("Teacher"))
                    && (u.FirstName.ToLower().Contains(searchText.ToLower())
                    || u.LastName.ToLower().Contains(searchText.ToLower())
                    || (u.FirstName + " " + u.LastName).ToLower().Contains(searchText.ToLower()))).ToListAsync();
            }
            else
            {
                teachers = await _context.Users
                    .Where(u => (!u.Deleted && u.Visible && u.Role.Equals("Teacher") && u.Id != currentUserId)
                    && (u.FirstName.ToLower().Contains(searchText.ToLower())
                    || u.LastName.ToLower().Contains(searchText.ToLower())
                    || (u.FirstName + " " + u.LastName).ToLower().Contains(searchText.ToLower()))).ToListAsync();
            }

            return teachers;
        }
        #endregion
	}
}
