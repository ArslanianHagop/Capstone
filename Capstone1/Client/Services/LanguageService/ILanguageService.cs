namespace Capstone1.Client.Services.LanguageService
{
    public interface ILanguageService
    {
        event Action OnChange;
        #region "Lists"
        List<Language> Languages { get; set; }
        List<Language> AdminLanguages { get; set; }
        #endregion
        #region "Get Methods"
        Task GetLanguages();
        Task GetAdminLanguages();
        #endregion
        #region "Post Methods"
        Task AddLanguage(Language language);
        #endregion
        #region "Put Methods"
        Task UpdateLanguage(Language language);
        #endregion
        #region "Delete Methods"
        Task DeleteLanguage(int id);
        #endregion
        Language CreateNewLanguage();
    }
}
