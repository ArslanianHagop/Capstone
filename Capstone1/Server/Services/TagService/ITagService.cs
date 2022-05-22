namespace Capstone1.Server.Services.TagService
{
    public interface ITagService
    {
        Task<ServiceResponse<List<Tag>>> GetPostsTags(int postId);
        Task<ServiceResponse<Tag>> AddTag(Tag tag);
        Task<ServiceResponse<Tag>> UpdateTag(Tag tag);
        Task<ServiceResponse<bool>> DeleteTag(int tagId);
    }
}
