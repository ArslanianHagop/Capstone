namespace Capstone1.Server.Services.TagService
{
    public class TagService : ITagService
    {
        private readonly DataContext _context;

        public TagService(DataContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<Tag>> AddTag(Tag tag)
        {
            _context.Tags.Add(tag);
            await _context.SaveChangesAsync();
            return new ServiceResponse<Tag> { Data = tag };
        }

        public async Task<ServiceResponse<bool>> DeleteTag(int tagId)
        {
            var dbTag = await _context.Tags.FindAsync(tagId);
            if(dbTag == null)
            {
                return new ServiceResponse<bool>
                {
                    Data = false,
                    Success = false,
                    Message = "Tag not found to delete."
                };
            }

            _context.Tags.Remove(dbTag);
            await _context.SaveChangesAsync();

            return new ServiceResponse<bool> { Data = true };
        }

        public async Task<ServiceResponse<List<Tag>>> GetPostsTags(int postId)
        {
            var result = await _context.Tags.Where(t => t.PostId == postId).ToListAsync();
            if(result == null && result.Count == 0)
            {
                return new ServiceResponse<List<Tag>>
                {
                    Success = false,
                    Message = "This post doesn't have any tags."
                };
            }

            return new ServiceResponse<List<Tag>> { Data = result };
        }

        public async Task<ServiceResponse<Tag>> UpdateTag(Tag tag)
        {
            var dbTag = await _context.Tags.FindAsync(tag.Id);
            if(dbTag == null)
            {
                return new ServiceResponse<Tag>
                {
                    Success = false,
                    Message = "Tag not found to update."
                };
            }

            dbTag.Name = tag.Name;

            await _context.SaveChangesAsync();

            return new ServiceResponse<Tag> { Data = tag };
        }
    }
}
