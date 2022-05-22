using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Capstone1.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProposalController : ControllerBase
    {
        private readonly IProposalService _proposalService;

        public ProposalController(IProposalService proposalService)
        {
            _proposalService = proposalService;
        }

        #region "Get Methods"
        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<Proposal>>> GetProposal(int id)
        {
            var response = await _proposalService.GetProposal(id);
            return Ok(response);
        }

        [HttpGet("current-teacher-proposals")]
        public async Task<ActionResult<ServiceResponse<List<Proposal>>>> GetLoggedInTeachersProposals()
        {
            var response = await _proposalService.GetLoggedInTeachersProposals();
            return Ok(response);
        }

        [HttpGet("all")]
        public async Task<ActionResult<ServiceResponse<List<Proposal>>>> GetAllProposals()
        {
            var response = await _proposalService.GetAllProposals();
            return Ok(response);
        }

        [HttpGet("teacher/{teacherId}")]
        public async Task<ActionResult<ServiceResponse<List<Proposal>>>> GetTeacherName(int teacherId)
        {
            var response = await _proposalService.GetTeacherName(teacherId);
            return Ok(response);
        }

        [HttpGet("teacherEmail/{teacherId}")]
        public async Task<ActionResult<ServiceResponse<List<Proposal>>>> GetTeacherEmail(int teacherId)
        {
            var response = await _proposalService.GetTeacherEmail(teacherId);
            return Ok(response);
        }

        [HttpGet("postsProposalsPaginated/{postId}/{page}")]
        public async Task<ActionResult<ServiceResponse<List<Proposal>>>> GetPostsProposalsPaginated(int postId, int page)
        {
            var response = await _proposalService.GetPostsProposalsPaginated(postId, page);
            return Ok(response);
        }

        [HttpGet("postsProposalsCount/{postId}")]
        public async Task<ActionResult<ServiceResponse<int>>> GetPostsProposalsCount(int postId)
        {
            var response = await _proposalService.GetPostsProposalsCount(postId);
            return Ok(response);
        }

        [HttpGet("loggedInTeachersProposalsPaginated/{page}")]
        public async Task<ActionResult<ServiceResponse<List<Proposal>>>> GetLoggedInTeachersProposalsPaginated(int page)
        {
            var response = await _proposalService.GetLoggedInTeachersProposalsPaginated(page);
            return Ok(response);
        }

        [HttpGet("admin/{id}/{page}")]
        public async Task<ActionResult<ServiceResponse<List<Proposal>>>> GetTeachersProposalsPaginated(int id, int page)
        {
            var response = await _proposalService.GetTeachersProposalsPaginated(id, page);
            return Ok(response);
        }

        [HttpGet("loggedInTeachersProposalsCount")]
        public async Task<ActionResult<ServiceResponse<int>>> GetLoggedInTeachersProposalsCount()
        {
            var response = await _proposalService.GetLoggedInTeachersProposalsCount();
            return Ok(response);
        }

        [HttpGet("admin/{id}")]
        public async Task<ActionResult<ServiceResponse<int>>> GetTeachersProposalsCount(int id)
        {
            var response = await _proposalService.GetTeachersProposalsCount(id);
            return Ok(response);
        }

        [HttpGet("admin/paginated/{page}")]
        public async Task<ActionResult<ServiceResponse<List<Proposal>>>> AdminGetProposalsPaginated(int page)
        {
            var response = await _proposalService.AdminGetProposalsPaginated(page);
            return Ok(response);
        }

        [HttpGet("admin/all/count")]
        public async Task<ActionResult<ServiceResponse<List<Proposal>>>> AdminGetProposalsCount()
        {
            var response = await _proposalService.AdminGetProposalsCount();
            return Ok(response);
        }

        [HttpGet("search/{searchText}/{page}")]
        public async Task<ActionResult<ServiceResponse<List<Proposal>>>> SearchProposals(string searchText, int page)
        {
            var response = await _proposalService.SearchProposals(searchText, page);
            return Ok(response);
        }

        [HttpGet("count/{searchText}")]
        public async Task<ActionResult<ServiceResponse<int>>> SearchProposalsCount(string searchText)
        {
            var response = await _proposalService.SearchProposalsCount(searchText);
            return Ok(response);
        }

        [HttpGet("teachersProposalOnPost/{postId}")]
        public async Task<ActionResult<ServiceResponse<Proposal>>> GetLoggedInTeachersProposalOnPost(int postId)
        {
            var response = await _proposalService.GetLoggedInTeachersProposalOnPost(postId);
            return Ok(response);
        }

        [HttpGet("acceptProposal/{proposalId}"), Authorize(Roles = "Student")]
        public async Task<ActionResult<ServiceResponse<Chat>>> AcceptProposal(int proposalId)
        {
            var response = await _proposalService.AcceptProposal(proposalId);
            return Ok(response);
        }

        [HttpGet("proposalsAuthorsProfilePics")]
        public async Task<ActionResult<ServiceResponse<Dictionary<int, string>>>> GetProposalsAuthorsProfielPics()
        {
            var response = await _proposalService.GetProposalsAuthorsProfilePics();
            return Ok(response);
        }

        [HttpGet("proposalsAuthors")]
        public async Task<ActionResult<ServiceResponse<Dictionary<int, string>>>> GetProposalsAuthors()
        {
            var response = await _proposalService.GetProposalsAuthors();
            return Ok(response);
        }
        #endregion

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<Proposal>>> AddProposal(Proposal proposal)
        {
            var response = await _proposalService.AddProposal(proposal);
            return Ok(response);
        }

        [HttpPut]
        public async Task<ActionResult<ServiceResponse<Proposal>>> UpdateProposal(Proposal proposal)
        {
            var response = await _proposalService.UpdateProposal(proposal);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<bool>>> DeleteProposal(int id)
        {
            var response = await _proposalService.DeleteProposal(id);
            return Ok(response);
        }
    }
}
