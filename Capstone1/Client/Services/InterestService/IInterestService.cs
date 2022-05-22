namespace Capstone1.Client.Services.InterestService
{
    public interface IInterestService
    {
        event Action OnChange;
        #region "Lists"
        List<Interest> Interests { get; set; }
        List<Interest> AdminInterests { get; set; }
        #endregion
        #region "Get Methods"
        Task GetInterests();
        Task GetAdminInterests();
        #endregion
        #region "Post Methods"
        Task AddInterest(Interest interest);
        #endregion
        #region "Put Methods"
        Task UpdateInterest(Interest interest);
        #endregion
        #region "Delete Methods"
        Task DeleteInterest(int id);
        #endregion
        Interest CreateNewInterest();
    }
}
