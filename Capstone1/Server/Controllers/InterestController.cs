using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Capstone1.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InterestController : ControllerBase
    {
        private readonly IInterestService _interestService;

        public InterestController(IInterestService interestService)
        {
            _interestService = interestService;
        }

        #region "Get Methods"
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<Interest>>>> GetInterests()
        {
            var response = await _interestService.GetInterests();
            return Ok(response);
        }

        [HttpGet("admin"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<List<Interest>>>> GetAdminInterests()
        {
            var response = await _interestService.GetAdminInterests();
            return Ok(response);
        }
        #endregion

        [HttpPost("admin"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<List<Interest>>>> AddInterest(Interest interest)
        {
            var response = await _interestService.AddInterest(interest);
            return Ok(response);
        }

        [HttpPut("admin"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<List<Interest>>>> UpdateInterest(Interest interest)
        {
            var response = await _interestService.UpdateInterest(interest);
            return Ok(response);
        }

        [HttpDelete("admin/{id}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<List<Interest>>>> DeleteInterest(int id)
        {
            var response = await _interestService.DeleteInterest(id);
            return Ok(response);
        }
    }
}
