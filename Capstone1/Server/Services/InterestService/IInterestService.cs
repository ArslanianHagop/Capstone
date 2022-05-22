namespace Capstone1.Server.Services.InterestService
{
    public interface IInterestService
    {
        #region "Get Methods"
        Task<ServiceResponse<List<Interest>>> GetInterests();
        Task<ServiceResponse<List<Interest>>> GetAdminInterests();
        #endregion
        #region "Post Methods"
        Task<ServiceResponse<List<Interest>>> AddInterest(Interest interest);
        #endregion
        #region "Put Methods"
        Task<ServiceResponse<List<Interest>>> UpdateInterest(Interest interest);
        #endregion
        #region "Delete Methods"
        Task<ServiceResponse<List<Interest>>> DeleteInterest(int id);
        #endregion
    }
}
