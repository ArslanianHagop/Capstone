namespace Capstone1.Server.Services.LanguageService
{
    public class LanguageService : ILanguageService
    {
        private readonly DataContext _context;

        public LanguageService(DataContext context)
        {
            _context = context;
        }

        #region "Get Methods"
        public async Task<ServiceResponse<List<Language>>> GetAdminLanguages()
        {
            var languages = await _context.Languages
                .Where(l => !l.Deleted).ToListAsync();

            return new ServiceResponse<List<Language>> { Data = languages };
        }

        public async Task<ServiceResponse<List<Language>>> GetLanguages()
        {
            var response = new ServiceResponse<List<Language>>
            {
                Data = await _context.Languages.Where(l => !l.Deleted && l.Visible).ToListAsync(),
            };

            return response;
        }
        #endregion

        #region "Post Methods"
        public async Task<ServiceResponse<List<Language>>> AddLanguage(Language language)
        {
            language.Editing = language.IsNew = false;
            _context.Languages.Add(language);
            await _context.SaveChangesAsync();
            return await GetAdminLanguages();
        }
        #endregion

        #region "Put Methods"
        public async Task<ServiceResponse<List<Language>>> UpdateLanguage(Language language)
        {
            Language dbLanguage = await GetLanguageById(language.Id);
            if(dbLanguage == null)
            {
                return new ServiceResponse<List<Language>>
                {
                    Success = false,
                    Message = "Language not found."
                };
            }

            dbLanguage.Name = language.Name;
            dbLanguage.Visible = language.Visible;

            await _context.SaveChangesAsync();

            return await GetAdminLanguages();
        }
        #endregion

        #region "Private Methods"
        private async Task<Language> GetLanguageById(int id)
        {
            return await _context.Languages.FirstOrDefaultAsync(l => l.Id == id);
        }
        #endregion

        #region "Delete Methods"
        public async Task<ServiceResponse<List<Language>>> DeleteLanguage(int id)
        {
            Language language = await GetLanguageById(id);
            if(language == null)
            {
                return new ServiceResponse<List<Language>>
                {
                    Success = false,
                    Message = "Language not found."
                };
            }

            language.Deleted = true;
            await _context.SaveChangesAsync();

            return await GetAdminLanguages();
        }
        #endregion
    }
}
