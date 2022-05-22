namespace Capstone1.Client.Services.ProposalService
{
    public class ProposalService : IProposalService
    {
        private readonly HttpClient _http;

        public ProposalService(HttpClient http)
        {
            _http = http;
        }

        public event Action ProposalsChanged;
        #region "Lists"
        public List<Proposal> AdminProposals { get; set; }
        #endregion

        #region "Get Methods"
        public async Task<List<Proposal>> GetAllProposals()
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<Proposal>>>("api/proposal/all");
            return result.Data;
        }

        public async Task<List<Proposal>> GetLoggedInTeachersProposals()
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<Proposal>>>("api/proposal/current-teacher-proposals");
            return result.Data;
        }

        public async Task<int> GetLoggedInTeachersProposalsCount()
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<int>>("api/proposal/loggedInTeachersProposalsCount");
            return result.Data;
        }

        public async Task<int> GetTeachersProposalsCount(int id)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<int>>($"api/proposal/admin/{id}");
            return result.Data;
        }

        public async Task<List<Proposal>> GetLoggedInTeachersProposalsPaginated(int page)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<Proposal>>>($"api/proposal/loggedInTeachersProposalsPaginated/{page}");
            return result.Data;
        }

        public async Task<List<Proposal>> GetTeachersProposalsPaginated(int id, int page)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<Proposal>>>($"api/proposal/admin/{id}/{page}");
            return result.Data;
        }

        public async Task<int> GetPostsProposalsCount(int postId)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<int>>($"api/proposal/postsProposalsCount/{postId}");
            return result.Data;
        }

        public async Task<ServiceResponse<List<Proposal>>> GetPostsProposalsPaginated(int postId, int page)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<Proposal>>>($"api/proposal/postsProposalsPaginated/{postId}/{page}");
            return result;
        }

        public async Task<ServiceResponse<Proposal>> GetProposal(int id)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<Proposal>>($"api/proposal/{id}");
            return result;
        }

        public async Task<string> GetTeacherEmail(int teacherId)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<string>>($"api/proposal/teacherEmail/{teacherId}");
            return result.Data;
        }

        public async Task<string> GetTeacherName(int teacherId)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<string>>($"api/proposal/teacher/{teacherId}");
            return result.Data;
        }

        public async Task<int> AdminGetProposalsCount()
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<int>>("api/proposal/admin/all/count");
            return result.Data;
        }

        public async Task AdminGetProposalsPaginated(int page)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<Proposal>>>($"api/proposal/admin/paginated/{page}");
            if (result != null && result.Data != null)
                AdminProposals = result.Data;
        }

        public async Task SearchProposals(string searchText, int page)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<Proposal>>>($"api/proposal/search/{searchText}/{page}");
            if (result != null && result.Data != null)
                AdminProposals = result.Data;

            ProposalsChanged.Invoke();
        }

        public async Task<int> SearchProposalsCount(string searchText)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<int>>($"api/proposal/count/{searchText}");
            return result.Data;
        }

        public async Task<Proposal> GetLoggedInTeachersProposalOnPost(int postId)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<Proposal>>($"api/proposal/teachersProposalOnPost/{postId}");
            return result.Data;
        }

        public async Task<Dictionary<int, string>> GetProposalsAuthorsProfilePics()
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<Dictionary<int, string>>>("api/proposal/proposalsAuthorsProfilePics");
            return result.Data;
        }

        public async Task<Dictionary<int, string>> GetProposalsAuthors()
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<Dictionary<int, string>>>("api/proposal/proposalsAuthors");
            return result.Data;
        }
        

        public async Task<Chat> AcceptProposal(int proposalId)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<Chat>>($"api/proposal/acceptProposal/{proposalId}");
            return result.Data;
        }
        #endregion

        #region "Post Methods"
        public async Task<Proposal> AddProposal(Proposal proposal)
        {
            var result = await _http.PostAsJsonAsync("api/proposal", proposal);
            return (await result.Content.ReadFromJsonAsync<ServiceResponse<Proposal>>()).Data;
        }
        #endregion

        #region "Delete Methods"
        public async Task DeleteProposal(int id)
        {
            var result = await _http.DeleteAsync($"api/proposal/{id}");
        }
        #endregion

        #region "Put Methods"
        public async Task<Proposal> UpdateProposal(Proposal proposal)
        {
            var result = await _http.PutAsJsonAsync("api/proposal", proposal);
            return (await result.Content.ReadFromJsonAsync<ServiceResponse<Proposal>>()).Data;
        }
        #endregion

    }
}
