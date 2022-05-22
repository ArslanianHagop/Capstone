namespace Capstone1.Client.Services.InterestService
{
    public class InterestService : IInterestService
    {
        private readonly HttpClient _http;

        public InterestService(HttpClient http)
        {
            _http = http;
        }

        #region "Lists"
        public List<Interest> Interests { get; set; }
        public List<Interest> AdminInterests { get; set; }
        #endregion

        public event Action OnChange;

        #region "Post Methods"
        public async Task AddInterest(Interest interest)
        {
            var result = await _http.PostAsJsonAsync("api/interest/admin", interest);
            AdminInterests = (await result.Content.ReadFromJsonAsync<ServiceResponse<List<Interest>>>()).Data;
            await GetInterests();
            OnChange.Invoke();
        }
        #endregion


        public Interest CreateNewInterest()
        {
            var newInterest = new Interest { IsNew = true, Editing = true };
            AdminInterests.Add(newInterest);
            OnChange.Invoke();
            return newInterest;
        }

        #region "Delete Methods"
        public async Task DeleteInterest(int id)
        {
            var response = await _http.DeleteAsync($"api/interest/admin/{id}");
            AdminInterests = (await response.Content.ReadFromJsonAsync<ServiceResponse<List<Interest>>>()).Data;
            await GetInterests();
            OnChange.Invoke();
        }
        #endregion

        #region "Get Methods"
        public async Task GetAdminInterests()
        {
            var response = await _http.GetFromJsonAsync<ServiceResponse<List<Interest>>>("api/interest/admin");
            if (response != null && response.Data != null)
                AdminInterests = response.Data;
        }

        public async Task GetInterests()
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<Interest>>>("api/interest");
            if (result != null && result.Data != null)
                Interests = result.Data;
        }
        #endregion

        #region "Put Methods"
        public async Task UpdateInterest(Interest interest)
        {
            var response = await _http.PutAsJsonAsync("api/interest/admin", interest);
            AdminInterests = (await response.Content.ReadFromJsonAsync<ServiceResponse<List<Interest>>>()).Data;
            await GetInterests();
            OnChange.Invoke();
        }
        #endregion
    }
}
