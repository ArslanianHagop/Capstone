namespace Capstone1.Server.Services.ProposalService
{
    public interface IProposalService
    {
        #region "Get Methods"
        Task<ServiceResponse<Proposal>> GetProposal(int id);
        Task<ServiceResponse<List<Proposal>>> GetLoggedInTeachersProposals();
        Task<ServiceResponse<List<Proposal>>> GetAllProposals();
        Task<ServiceResponse<string>> GetTeacherName(int teacherId);
        Task<ServiceResponse<string>> GetTeacherEmail(int teacherId);
        Task<ServiceResponse<List<Proposal>>> GetPostsProposalsPaginated(int postId, int page);
        Task<ServiceResponse<int>> GetPostsProposalsCount(int postId);
        Task<ServiceResponse<List<Proposal>>> GetLoggedInTeachersProposalsPaginated(int page);
        Task<ServiceResponse<int>> GetLoggedInTeachersProposalsCount();
        Task<ServiceResponse<List<Proposal>>> GetTeachersProposalsPaginated(int id, int page);
        Task<ServiceResponse<int>> GetTeachersProposalsCount(int id);
        Task<ServiceResponse<int>> AdminGetProposalsCount();
        Task<ServiceResponse<List<Proposal>>> AdminGetProposalsPaginated(int page);
        Task<ServiceResponse<List<Proposal>>> SearchProposals(string searchText, int page);
        Task<ServiceResponse<int>> SearchProposalsCount(string searchText);
        Task<ServiceResponse<Proposal>> GetLoggedInTeachersProposalOnPost(int postId);
        Task<ServiceResponse<Dictionary<int, string>>> GetProposalsAuthorsProfilePics();
        Task<ServiceResponse<Dictionary<int, string>>> GetProposalsAuthors();
        Task<ServiceResponse<Chat>> AcceptProposal(int proposalId);
        #endregion
        #region "Post Methods"
        Task<ServiceResponse<Proposal>> AddProposal(Proposal proposal);
        #endregion
        #region "Put Methods"
        Task<ServiceResponse<Proposal>> UpdateProposal(Proposal proposal);
        #endregion
        #region "Delete Methods"
        Task<ServiceResponse<bool>> DeleteProposal(int id);
        #endregion
    }
}
