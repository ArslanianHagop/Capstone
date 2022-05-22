namespace Capstone1.Server.Services.LanguageService
{
    public interface ILanguageService
    {
        #region "Get Methods"
        Task<ServiceResponse<List<Language>>> GetLanguages();
        Task<ServiceResponse<List<Language>>> GetAdminLanguages();
        #endregion
        #region "Post Methods"
        Task<ServiceResponse<List<Language>>> AddLanguage(Language language);
        #endregion
        #region "Put Methods"
        Task<ServiceResponse<List<Language>>> UpdateLanguage(Language language);
        #endregion
        #region "Delete Methods"
        Task<ServiceResponse<List<Language>>> DeleteLanguage(int id);
        #endregion
    }
}
