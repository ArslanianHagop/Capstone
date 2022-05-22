namespace Capstone1.Client.Services.LanguageService
{
    public class LanguageService : ILanguageService
    {
        private readonly HttpClient _http;

        public LanguageService(HttpClient http)
        {
            _http = http;
        }

        #region "Lists"
        public List<Language> Languages { get; set; }
        public List<Language> AdminLanguages { get; set; }
        #endregion


        public event Action OnChange;

        #region "Get Methods"
        public async Task GetAdminLanguages()
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<Language>>>("api/language/admin");
            if (result != null && result.Data != null)
                AdminLanguages = result.Data;
        }

        public async Task GetLanguages()
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<Language>>>("api/language");
            if (result != null && result.Data != null)
                Languages = result.Data;
        }
        #endregion

        #region "Post Methods"
        public async Task AddLanguage(Language language)
        {
            var response = await _http.PostAsJsonAsync("api/language/admin", language);
            AdminLanguages = (await response.Content.ReadFromJsonAsync<ServiceResponse<List<Language>>>()).Data;
            await GetLanguages();
            OnChange.Invoke();
        }
        #endregion


        public Language CreateNewLanguage()
        {
            var newLanguage = new Language { IsNew = true, Editing = true };
            AdminLanguages.Add(newLanguage);
            OnChange.Invoke();
            return newLanguage;
        }

        #region "Delete Methods"
        public async Task DeleteLanguage(int id)
        {
            var response = await _http.DeleteAsync($"api/language/admin/{id}");
            AdminLanguages = (await response.Content.ReadFromJsonAsync<ServiceResponse<List<Language>>>()).Data;
            await GetLanguages();
            OnChange.Invoke();
        }
        #endregion

        #region "Put Methods"
        public async Task UpdateLanguage(Language language)
        {
            var response = await _http.PutAsJsonAsync("api/language/admin", language);
            AdminLanguages = (await response.Content.ReadFromJsonAsync<ServiceResponse<List<Language>>>()).Data;
            await GetLanguages();
            OnChange.Invoke();
        }
        #endregion
    }
}
