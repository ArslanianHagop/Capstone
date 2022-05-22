using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Capstone1.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        #region "Get Methods"
        [HttpGet("{studentId}")]
        public async Task<ActionResult<ServiceResponse<User>>> GetStudent(int studentId)
		{
            var response = await _studentService.GetStudent(studentId);
            return Ok(response);
		}

        [HttpGet("languages/{studentId}")]
        public async Task<ActionResult<ServiceResponse<UserLanguage>>> GetStudentsLanguages(int studentId)
        {
            var response = await _studentService.GetStudentsLanguages(studentId);
            return Ok(response);
        }

        [HttpGet("interests/{studentId}")]
        public async Task<ActionResult<ServiceResponse<StudentInterest>>> GetStudentsInterests(int studentId)
        {
            var response = await _studentService.GetStudentsInterests(studentId);
            return Ok(response);
        }

        [HttpGet("studentInterests")]
        public async Task<ActionResult<ServiceResponse<StudentInterest>>> GetAllStudentInterests()
        {
            var response = await _studentService.GetAllStudentInterests();
            return Ok(response);
        }

        [HttpGet("role-check")]
        public async Task<ActionResult<ServiceResponse<bool>>> StudentRoleCheck()
        {
            var response = await _studentService.StudentRoleCheck();
            return Ok(response);
        }

        [HttpGet("current-student-interests")]
        public async Task<ActionResult<ServiceResponse<List<StudentInterest>>>> GetLoggedInStudentsInterests()
        {
            var response = await _studentService.GetLoggedInStudentsInterests();
            return Ok(response);
        }

        [HttpGet("count")]
        public async Task<ActionResult<ServiceResponse<int>>> GetStudentsCount()
        {
            var response = await _studentService.GetStudentsCount();
            return Ok(response);
        }

        [HttpGet("paginated/{page}")]
        public async Task<ActionResult<ServiceResponse<List<User>>>> GetStudentsPaginated(int page)
        {
            var response = await _studentService.GetStudentsPaginated(page);
            return Ok(response);
        }

        [HttpGet("search/{searchText}/{page}")]
        public async Task<ActionResult<ServiceResponse<List<User>>>> SearchStudents(string searchText, int page)
        {
            var response = await _studentService.SearchStudents(searchText, page);
            return Ok(response);
        }

        [HttpGet("searchsuggestions/{searchText}")]
        public async Task<ActionResult<ServiceResponse<List<string>>>> GetStudentSearchSuggestions(string searchText)
        {
            var response = await _studentService.GetStudentSearchSuggestions(searchText);
            return Ok(response);
        }

        [HttpGet("count/{searchText}")]
        public async Task<ActionResult<ServiceResponse<int>>> SearchStudentsCount(string searchText)
        {
            var response = await _studentService.SearchStudentsCount(searchText);
            return Ok(response);
        }
        #endregion

        [HttpPut]
        public async Task<ActionResult<ServiceResponse<bool>>> ChangeInterests(Student student)
        {
            var response = await _studentService.ChangeInterests(student);
            return Ok(response);
        }
    }
}
