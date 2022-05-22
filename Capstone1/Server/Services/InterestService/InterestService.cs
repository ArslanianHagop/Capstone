namespace Capstone1.Server.Services.InterestService
{
    public class InterestService : IInterestService
    {
        private readonly DataContext _context;

        public InterestService(DataContext context)
        {
            _context = context;
        }

        #region "Get Methods"
        public async Task<ServiceResponse<List<Interest>>> GetAdminInterests()
        {
            var interests = await _context.Interests
                .Where(i => !i.Deleted).ToListAsync();
            return new ServiceResponse<List<Interest>> { Data = interests };
        }

        public async Task<ServiceResponse<List<Interest>>> GetInterests()
        {
            var interests = await _context.Interests
                .Where(i => !i.Deleted && i.Visible).ToListAsync();
            return new ServiceResponse<List<Interest>>
            {
                Data = interests
            };
        }
        #endregion

        #region "Post Methods"
        public async Task<ServiceResponse<List<Interest>>> AddInterest(Interest interest)
        {
            interest.Editing = interest.IsNew = false;
            _context.Interests.Add(interest);
            await _context.SaveChangesAsync();
            return await GetAdminInterests();
        }
        #endregion

        #region "Put Methods"
        public async Task<ServiceResponse<List<Interest>>> UpdateInterest(Interest interest)
        {
            var dbInterest = await GetInterestById(interest.Id);
            if(dbInterest == null)
            {
                return new ServiceResponse<List<Interest>>
                {
                    Success = false,
                    Message = "Interest not found."
                };
            }

            dbInterest.Name = interest.Name;
            dbInterest.Visible = interest.Visible;

            await _context.SaveChangesAsync();

            return await GetAdminInterests();
        }
        #endregion

        #region "Delete Methods"
        public async Task<ServiceResponse<List<Interest>>> DeleteInterest(int id)
        {
            Interest interest = await GetInterestById(id);
            if(interest == null)
            {
                return new ServiceResponse<List<Interest>>
                {
                    Success = false,
                    Message = "interest not found."
                };
            }

            interest.Deleted = true;
            await _context.SaveChangesAsync();

            return await GetAdminInterests();
        }
        #endregion

        #region "Private Methods"
        private async Task<Interest> GetInterestById(int id)
        {
            return await _context.Interests.FirstOrDefaultAsync(i => i.Id == id);
        }
        #endregion

    }
}
