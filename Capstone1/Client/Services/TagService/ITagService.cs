namespace Capstone1.Client.Services.TagService
{
    public interface ITagService
    {
        List<Tag> PostsTags { get; set; }
        Task GetPostsTags(int postId);
        Task<Tag> AddTag(Tag tag);
        Task<Tag> UpdateTag(Tag tag);
        Task DeleteTag(Tag tag);
    }
}
