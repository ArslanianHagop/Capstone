using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Capstone1.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherService _teacherService;

        public TeacherController(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        #region "Get Methods"
        [HttpGet("{teacherId}")]
        public async Task<ActionResult<ServiceResponse<User>>> GetTeacher(int teacherId)
        {
            var response = await _teacherService.GetTeacher(teacherId);
            return Ok(response);
        }

        [HttpGet("interests/{teacherId}")]
        public async Task<ActionResult<ServiceResponse<List<TeacherInterest>>>> GetTeachersInterests(int teacherId)
        {
            var response = await _teacherService.GetTeachersInterests(teacherId);
            return Ok(response);
        }

        [HttpGet("teacherJobs/{teacherId}")]
        public async Task<ActionResult<ServiceResponse<List<TeacherJob>>>> GetTeachersJobs(int teacherId)
        {
            var response = await _teacherService.GetTeachersJobs(teacherId);
            return Ok(response);
        }

        [HttpGet("teacherCourses/{teacherId}")]
        public async Task<ActionResult<ServiceResponse<List<TeacherCourse>>>> GetTeachersCourses(int teacherId)
        {
            var response = await _teacherService.GetTeachersCourses(teacherId);
            return Ok(response);
        }

        [HttpGet("languages/{teacherId}")]
        public async Task<ActionResult<ServiceResponse<List<UserLanguage>>>> GetTeachersLanguages(int teacherId)
        {
            var response = await _teacherService.GetTeachersLanguages(teacherId);
            return Ok(response);
        }

        [HttpGet("teacherInterests")]
        public async Task<ActionResult<ServiceResponse<List<TeacherInterest>>>> GetAllTeacherInterests()
        {
            var response = await _teacherService.GetAllTeacherInterests();
            return Ok(response);
        }

        [HttpGet("role-check")]
        public async Task<ActionResult<ServiceResponse<bool>>> TeacherRoleCheck()
        {
            var response = await _teacherService.TeacherRoleCheck();
            return Ok(response);
        }

        [HttpGet("current-teacher-interests"), Authorize(Roles = "Teacher")]
        public async Task<ActionResult<ServiceResponse<List<TeacherInterest>>>> GetLoggedInTeachersInterests()
        {
            var response = await _teacherService.GetLoggedInTeachersInterests();
            return Ok(response);
        }

        [HttpGet("current-teacher-jobs"), Authorize(Roles = "Teacher")]
        public async Task<ActionResult<ServiceResponse<List<TeacherJob>>>> GetLoggedInTeachersJobs()
        {
            var response = await _teacherService.GetLoggedInTeachersJobs();
            return Ok(response);
        }

        [HttpGet("current-teacher-courses"), Authorize(Roles = "Teacher")]
        public async Task<ActionResult<ServiceResponse<List<TeacherCourse>>>> GetLoggedInTeachersCourse()
        {
            var response = await _teacherService.GetLoggedInTeachersCourses();
            return Ok(response);
        }

        [HttpGet("all-teacher-course-interests")]
        public async Task<ActionResult<ServiceResponse<List<TeacherCourseInterest>>>> GetAllTeacherCourseInterests()
        {
            var response = await _teacherService.GetAllTeacherCourseInterests();
            return Ok(response);
        }

        [HttpGet("random/{num}")]
        public async Task<ActionResult<ServiceResponse<List<User>>>> RandomTeachers(int num)
        {
            var response = await _teacherService.RandomTeachers(num);
            return Ok(response);
        }

        [HttpGet("paginated/{page}")]
        public async Task<ActionResult<ServiceResponse<List<User>>>> GetTeachersPaginated(int page)
        {
            var response = await _teacherService.GetTeachersPaginated(page);
            return Ok(response);
        }

        [HttpGet("count")]
        public async Task<ActionResult<ServiceResponse<int>>> GetTeachersCount()
        {
            var response = await _teacherService.GetTeachersCount();
            return Ok(response);
        }

        [HttpGet("search/{searchText}/{page}")]
        public async Task<ActionResult<ServiceResponse<List<User>>>> SearchTeachers(string searchText, int page)
        {
            var response = await _teacherService.SearchTeachers(searchText, page);
            return Ok(response);
        }

        [HttpGet("searchsuggestions/{searchText}")]
        public async Task<ActionResult<ServiceResponse<List<string>>>> GetTeacherSearchSuggestions(string searchText)
        {
            var response = await _teacherService.GetTeacherSearchSuggestions(searchText);
            return Ok(response);
        }

        [HttpGet("count/{searchText}")]
        public async Task<ActionResult<ServiceResponse<int>>> SearchTeachersCount(string searchText)
        {
            var response = await _teacherService.SearchTeachersCount(searchText);
            return Ok(response);
        }
        #endregion

        #region "Post Methods"
        [HttpPost("teacher-job"), Authorize(Roles = "Teacher")]
        public async Task<ActionResult<ServiceResponse<List<TeacherJob>>>> AddTeacherJob(TeacherJob teacherJob)
        {
            var response = await _teacherService.AddTeacherJob(teacherJob);
            return Ok(response);
        }

        [HttpPost("teacher-course"), Authorize(Roles = "Teacher")]
        public async Task<ActionResult<ServiceResponse<List<TeacherCourse>>>> AddTeacherCourse(TeacherCourse teacherCourse)
        {
            var response = await _teacherService.AddTeacherCourse(teacherCourse);
            return Ok(response);
        }
        #endregion

        #region "Put Methods"
        [HttpPut("teacher-job"), Authorize]
        public async Task<ActionResult<ServiceResponse<List<TeacherJob>>>> UpdateTeacherJob(TeacherJob teacherJob)
        {
            var response = await _teacherService.UpdateTeacherJob(teacherJob);
            return Ok(response);
        }

        [HttpPut("teacher-course"), Authorize]
        public async Task<ActionResult<ServiceResponse<List<TeacherCourse>>>> UpdateTeacherCourse(TeacherCourse teacherCourse)
        {
            var response = await _teacherService.UpdateTeacherCourse(teacherCourse);
            return Ok(response);
        }

        [HttpPut]
        public async Task<ActionResult<ServiceResponse<bool>>> ChangeInterests(Teacher teacher)
        {
            var response = await _teacherService.ChangeInterests(teacher);
            return Ok(response);
        }
        #endregion

        #region "Delete Methods"
        [HttpDelete("teacher-job/{id}"), Authorize(Roles = "Teacher")]
        public async Task<ActionResult<ServiceResponse<List<TeacherJob>>>> DeleteTeacherJob(int id)
        {
            var response = await _teacherService.DeleteTeacherJob(id);
            return Ok(response);
        }

        [HttpDelete("teacher-course/{id}"), Authorize(Roles = "Teacher")]
        public async Task<ActionResult<ServiceResponse<List<TeacherCourse>>>> DeleteTeacherCourse(int id)
        {
            var response = await _teacherService.DeleteTeacherCourse(id);
            return Ok(response);
        }
        #endregion
    }
}
