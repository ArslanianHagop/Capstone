using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Capstone1.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LanguageController : ControllerBase
    {
        private readonly ILanguageService _languageService;

        public LanguageController(ILanguageService languageService)
        {
            _languageService = languageService;
        }

        #region "Get Methods"
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<Language>>>> GetLanguages()
        {
            var result = await _languageService.GetLanguages();
            return Ok(result);
        }

        [HttpGet("admin"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<List<Language>>>> GetAdminLanguages()
        {
            var result = await _languageService.GetAdminLanguages();
            return Ok(result);
        }
        #endregion

        [HttpPost("admin"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<List<Language>>>> AddLanguage(Language language)
        {
            var result = await _languageService.AddLanguage(language);
            return Ok(result);
        }

        [HttpPut("admin"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<List<Language>>>> UpdateLanguage(Language language)
        {
            var result = await _languageService.UpdateLanguage(language);
            return Ok(result);
        }

        [HttpDelete("admin/{id}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceResponse<List<Language>>>> DeleteLanguage(int id)
        {
            var result = await _languageService.DeleteLanguage(id);
            return Ok(result);
        }
    }
}
