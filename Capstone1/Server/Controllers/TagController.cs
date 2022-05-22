using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Capstone1.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly ITagService _tagService;

        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }

        [HttpGet("{postId}")]
        public async Task<ActionResult<ServiceResponse<List<Post>>>> GetPostsTags(int postId)
        {
            var result = await _tagService.GetPostsTags(postId);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<Tag>>> AddTag(Tag tag)
        {
            var result = await _tagService.AddTag(tag);
            return Ok(result);
        }

        [HttpPut]
        public async Task<ActionResult<ServiceResponse<Tag>>> UpdateTag(Tag tag)
        {
            var result = await _tagService.UpdateTag(tag);
            return Ok(result);
        }

        [HttpDelete("{tagId}")]
        public async Task<ActionResult<ServiceResponse<bool>>> DeleteTag(int tagId)
        {
            var result = await _tagService.DeleteTag(tagId);
            return Ok(result);
        }
    }
}
