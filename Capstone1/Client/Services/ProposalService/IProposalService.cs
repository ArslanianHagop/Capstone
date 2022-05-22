namespace Capstone1.Client.Services.ProposalService
{
    public interface IProposalService
    {
        event Action ProposalsChanged;
        #region "Lists"
        List<Proposal> AdminProposals { get; set; }
        #endregion
        #region "Get Methods"
        Task<ServiceResponse<Proposal>> GetProposal(int id);
        Task<List<Proposal>> GetLoggedInTeachersProposals();
        Task<List<Proposal>> GetAllProposals();
        Task<string> GetTeacherName(int teacherId);
        Task<string> GetTeacherEmail(int teacherId);
        Task<ServiceResponse<List<Proposal>>> GetPostsProposalsPaginated(int postId, int page);
        Task<int> GetPostsProposalsCount(int postId);
        Task<List<Proposal>> GetLoggedInTeachersProposalsPaginated(int page);
        Task<int> GetLoggedInTeachersProposalsCount();
        Task<List<Proposal>> GetTeachersProposalsPaginated(int id, int page);
        Task<int> GetTeachersProposalsCount(int id);
        Task<int> AdminGetProposalsCount();
        Task AdminGetProposalsPaginated(int page);
        Task SearchProposals(string searchText, int page);
        Task<int> SearchProposalsCount(string searchText);
        Task<Proposal> GetLoggedInTeachersProposalOnPost(int postId);
        Task<Dictionary<int, string>> GetProposalsAuthorsProfilePics();
        Task<Dictionary<int, string>> GetProposalsAuthors();
        Task<Chat> AcceptProposal(int proposalId);
        #endregion
        #region "Post Methods"
        Task<Proposal> AddProposal(Proposal proposal);
        #endregion
        #region "Put Methods"
        Task<Proposal> UpdateProposal(Proposal proposal);
        #endregion
        #region "Delete Methods"
        Task DeleteProposal(int id);
        #endregion
    }
}
