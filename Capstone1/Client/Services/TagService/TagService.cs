namespace Capstone1.Client.Services.TagService
{
    public class TagService : ITagService
    {
        private readonly HttpClient _http;

        public TagService(HttpClient http)
        {
            _http = http;
        }

        public List<Tag> PostsTags { get; set; }

        public async Task<Tag> AddTag(Tag tag)
        {
            var result = await _http.PostAsJsonAsync("api/tag", tag);
            return (await result.Content.ReadFromJsonAsync<ServiceResponse<Tag>>()).Data;
        }

        public async Task DeleteTag(Tag tag)
        {
            var result = await _http.DeleteAsync($"api/tag/{tag.Id}");
        }

        public async Task GetPostsTags(int postId)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<Tag>>>($"api/tag/{postId}");
            if (result != null && result.Data != null)
                PostsTags = result.Data;
        }

        public async Task<Tag> UpdateTag(Tag tag)
        {
            var result = await _http.PutAsJsonAsync("api/tag", tag);
            return (await result.Content.ReadFromJsonAsync<ServiceResponse<Tag>>()).Data;
        }
    }
}
