namespace Capstone1.Server.Services.ProposalService
{
    public class ProposalService : IProposalService
    {
        private readonly DataContext _context;
        private readonly IAuthService _authService;
        private readonly IPostService _postService;

        public ProposalService(DataContext context, IAuthService authService, IPostService postService)
        {
            _context = context;
            _authService = authService;
            _postService = postService;
        }

        #region "Get Methods"
        public async Task<ServiceResponse<List<Proposal>>> GetAllProposals()
        {
            bool isAdmin = (await _authService.IsAdmin()).Data;
            List<Proposal> proposals = await _context.Proposals.Where(p => !p.Deleted && (p.Visible || isAdmin)).ToListAsync();
            if (proposals == null)
            {
                return new ServiceResponse<List<Proposal>>
                {
                    Success = false,
                    Message = "No Proposals have been made yet."
                };
            }

            return new ServiceResponse<List<Proposal>> { Data = proposals };
        }

        public async Task<ServiceResponse<List<Proposal>>> GetLoggedInTeachersProposals()
        {
            var currentUserId = await _authService.GetUserId();
            List<Proposal> proposals = await _context.Proposals.Where(p => p.TeacherId == currentUserId && !p.Deleted && p.Visible).ToListAsync();
            if (proposals == null || proposals.Count == 0)
            {
                return new ServiceResponse<List<Proposal>>
                {
                    Success = false,
                    Message = "This teacher doesn't have any proposals."
                };
            }

            return new ServiceResponse<List<Proposal>> { Data = proposals };
        }

        public async Task<ServiceResponse<int>> GetLoggedInTeachersProposalsCount()
        {
            var currentUserId = await _authService.GetUserId();
            bool isAdmin = (await _authService.IsAdmin()).Data;
            var proposalsCount = await _context.Proposals.Where(p => p.TeacherId == currentUserId && !p.Deleted && (p.Visible || isAdmin)).CountAsync();
            return new ServiceResponse<int> { Data = proposalsCount };
        }

        public async Task<ServiceResponse<int>> GetTeachersProposalsCount(int id)
        {
            bool isAdmin = (await _authService.IsAdmin()).Data;
            var proposalsCount = await _context.Proposals.Where(p => p.TeacherId == id && !p.Deleted && !p.IsAccepted && (p.Visible || isAdmin)).CountAsync();
            return new ServiceResponse<int> { Data = proposalsCount };
        }

        public async Task<ServiceResponse<List<Proposal>>> GetLoggedInTeachersProposalsPaginated(int page)
        {
            var currentUserId = await _authService.GetUserId();
            bool isAdmin = (await _authService.IsAdmin()).Data;
            var pageResults = 5f;
            var proposalsPaginated = await _context.Proposals
                .Where(p => p.TeacherId == currentUserId && !p.Deleted && (p.Visible || isAdmin)).Skip((page - 1) * (int)pageResults)
                .Take((int)pageResults).ToListAsync();

            if (proposalsPaginated == null || proposalsPaginated.Count == 0)
            {
                return new ServiceResponse<List<Proposal>>
                {
                    Success = false,
                    Message = "This teacher has no proposals."
                };
            }

            return new ServiceResponse<List<Proposal>> { Data = proposalsPaginated };
        }

        public async Task<ServiceResponse<List<Proposal>>> GetTeachersProposalsPaginated(int id, int page)
        {
            bool isAdmin = (await _authService.IsAdmin()).Data;
            var pageResults = 5f;
            var proposalsPaginated = await _context.Proposals
                .Where(p => p.TeacherId == id && !p.Deleted && !p.IsAccepted && (p.Visible || isAdmin)).Skip((page - 1) * (int)pageResults)
                .Take((int)pageResults).ToListAsync();

            if (proposalsPaginated == null || proposalsPaginated.Count == 0)
            {
                return new ServiceResponse<List<Proposal>>
                {
                    Success = false,
                    Message = "This teacher has no proposals."
                };
            }

            return new ServiceResponse<List<Proposal>> { Data = proposalsPaginated };
        }

        public async Task<ServiceResponse<int>> GetPostsProposalsCount(int postId)
        {
            bool isAdmin = (await _authService.IsAdmin()).Data;
            var proposalsCount = await _context.Proposals.Where(p => p.PostId == postId && !p.Deleted && (p.Visible || isAdmin)).CountAsync();
            return new ServiceResponse<int> { Data = proposalsCount };
        }

        public async Task<ServiceResponse<List<Proposal>>> GetPostsProposalsPaginated(int postId, int page)
        {
            bool isAdmin = (await _authService.IsAdmin()).Data;
            var pageResults = 5f;
            List<Proposal> proposalsPaginated = await _context.Proposals.Where(p => p.PostId == postId && !p.Deleted && (p.Visible || isAdmin)).Skip((page - 1) * (int)pageResults).Take((int)pageResults).ToListAsync();
            if (proposalsPaginated == null || proposalsPaginated.Count == 0)
            {
                return new ServiceResponse<List<Proposal>>
                {
                    Success = false,
                    Message = "This post has no proposals."
                };
            }

            return new ServiceResponse<List<Proposal>> { Data = proposalsPaginated };
        }

        public async Task<ServiceResponse<Proposal>> GetProposal(int id)
        {
            bool isAdmin = (await _authService.IsAdmin()).Data;
            var proposal = await _context.Proposals.FirstOrDefaultAsync(p => p.Id == id && !p.Deleted && (p.Visible || isAdmin));
            if (proposal == null)
            {
                return new ServiceResponse<Proposal>
                {
                    Success = false,
                    Message = "Sorry, this proposal does not exist."
                };
            }

            return new ServiceResponse<Proposal> { Data = proposal };
        }

        public async Task<ServiceResponse<string>> GetTeacherEmail(int teacherId)
        {
            var teacher = await _context.Users.FirstOrDefaultAsync(u => u.Id == teacherId && !u.Deleted && u.Visible);
            if (teacher == null)
            {
                return new ServiceResponse<string>
                {
                    Success = false,
                    Message = "This teacher doesn't exist."
                };
            }

            return new ServiceResponse<string> { Data = teacher.Email };
        }

        public async Task<ServiceResponse<string>> GetTeacherName(int teacherId)
        {
            var teacher = await _context.Users.FirstOrDefaultAsync(u => u.Id == teacherId && !u.Deleted && (u.Visible || u.Role.Equals("Admin")));
            if (teacher == null)
            {
                return new ServiceResponse<string>
                {
                    Success = false,
                    Message = "This teacher doesn't exist."
                };
            }

            string fullName = teacher.FirstName + " " + teacher.LastName;
            return new ServiceResponse<string> { Data = fullName };
        }

        public async Task<ServiceResponse<int>> AdminGetProposalsCount()
        {
            int count = await _context.Proposals.Where(p => !p.Deleted).CountAsync();
            return new ServiceResponse<int> { Data = count };
        }

        public async Task<ServiceResponse<List<Proposal>>> AdminGetProposalsPaginated(int page)
        {
            var pageResults = 5f;
            var adminProposals = await _context.Proposals
                .Where(p => !p.Deleted)
                .Skip((page - 1) * (int)pageResults).Take((int)pageResults).ToListAsync();
            if (adminProposals == null || adminProposals.Count == 0)
            {
                return new ServiceResponse<List<Proposal>>
                {
                    Success = false,
                    Message = "No proposals are made yet."
                };
            }

            return new ServiceResponse<List<Proposal>> { Data = adminProposals };
        }

        public async Task<ServiceResponse<List<Proposal>>> SearchProposals(string searchText, int page)
        {
            var pageResults = 5f;
            var proposals = await FindProposalsBySearchText(searchText);
            var proposalsPaginated = proposals.Skip((page - 1) * (int)pageResults).Take((int)pageResults).ToList();

            return new ServiceResponse<List<Proposal>> { Data = proposalsPaginated };
        }

        public async Task<ServiceResponse<int>> SearchProposalsCount(string searchText)
        {
            return new ServiceResponse<int> { Data = (await FindProposalsBySearchText(searchText)).Count };
        }

        public async Task<ServiceResponse<Proposal>> GetLoggedInTeachersProposalOnPost(int postId)
        {
            var currentUserId = await _authService.GetUserId();
            var proposal = await _context.Proposals
                .FirstOrDefaultAsync(p => !p.Deleted && p.Visible && p.PostId == postId
                && p.TeacherId == currentUserId);
            if (proposal == null)
            {
                return new ServiceResponse<Proposal>
                {
                    Success = false,
                    Message = "This teacher hasn't made a proposal on this post."
                };
            }

            return new ServiceResponse<Proposal> { Data = proposal };
        }

        public async Task<ServiceResponse<Dictionary<int, string>>> GetProposalsAuthorsProfilePics()
        {
            var proposals = await _context.Proposals.Where(p => !p.Deleted).ToListAsync();
            Dictionary<int, string> result = new Dictionary<int, string>();

            foreach (var proposal in proposals)
            {
                var proposalsTeacher = await _context.Users.FindAsync(proposal.TeacherId);
                if (proposalsTeacher != null)
                    result.Add(proposal.Id, proposalsTeacher.ProfilePic);
            }

            if(result == null || result.Count == 0)
            {
                return new ServiceResponse<Dictionary<int, string>>
                {
                    Success = false,
                    Message = "There hasn't been any proposals to find their authors profile pics."
                };
            }

            return new ServiceResponse<Dictionary<int, string>> { Data = result };
        }

        public async Task<ServiceResponse<Dictionary<int, string>>> GetProposalsAuthors()
        {
            var proposals = await _context.Proposals.Where(p => !p.Deleted).ToListAsync();
            Dictionary<int, string> result = new Dictionary<int, string>();

            foreach (var proposal in proposals)
            {
                var proposalsTeacher = await _context.Users.FindAsync(proposal.TeacherId);
                if(proposalsTeacher != null)
                    result.Add(proposal.Id, proposalsTeacher.FirstName + " " + proposalsTeacher.LastName);
            }

            if(result == null || result.Count == 0)
            {
                return new ServiceResponse<Dictionary<int, string>>
                {
                    Success = false,
                    Message = "There hasn't been any proposals to find their authors."
                };
            }

            return new ServiceResponse<Dictionary<int, string>> { Data = result };
        }

        public async Task<ServiceResponse<Chat>> AcceptProposal(int proposalId)
        {
            var proposal = await _context.Proposals.FindAsync(proposalId);
            var acceptanceMessage = new Chat();
            if(proposal == null)
            {
                return new ServiceResponse<Chat>
                {
                    Success = false,
                    Message = "Proposal was not found to accept."
                };
            }

            var result = await _postService.DeletePost(proposal.PostId);
            if(result != null && result.Data != null)
			{
                result.Data.IsAccepted = true;
                result.Data.Deleted = false;
			}
            proposal.Deleted = false;
            proposal.IsAccepted = true;
            if (result.Success)
            {
                acceptanceMessage = new Chat
                {
                    SenderId = await _authService.GetUserId(),
                    RecipientId = proposal.TeacherId,
                    Message = $"I have accepted your proposal on my post '{result.Data.Title}'"
                };
                _context.Chats.Add(acceptanceMessage);
                await _context.SaveChangesAsync();
            }

            return new ServiceResponse<Chat> { Data = acceptanceMessage };
        }
        #endregion

        #region "Post Methods"
        public async Task<ServiceResponse<Proposal>> AddProposal(Proposal proposal)
        {
            proposal.Editing = proposal.IsNew = false;
            proposal.TeacherId = await _authService.GetUserId();
            if (!(await _context.Proposals.AnyAsync(p => p.TeacherId == proposal.TeacherId && !p.Deleted && p.Visible && p.PostId == proposal.PostId)))
                _context.Proposals.Add(proposal);
            await _context.SaveChangesAsync();
            return new ServiceResponse<Proposal> { Data = proposal };
        }
        #endregion

        #region "Put Methods"
        public async Task<ServiceResponse<Proposal>> UpdateProposal(Proposal proposal)
        {
            bool isAdmin = (await _authService.IsAdmin()).Data;
            var dbProposal = await _context.Proposals.FirstOrDefaultAsync(p => p.Id == proposal.Id && !p.Deleted && (p.Visible || isAdmin));
            if (dbProposal == null)
            {
                return new ServiceResponse<Proposal>
                {
                    Success = false,
                    Message = "Proposal not found to update."
                };
            }

            dbProposal.Description = proposal.Description;
            dbProposal.Budget = proposal.Budget;
            var dbTeacher = await _context.Users.FirstOrDefaultAsync(u => u.Id == dbProposal.TeacherId);
            var dbPost = await _context.Posts.FirstOrDefaultAsync(p => p.Id == dbProposal.PostId);
            if (dbTeacher.Visible && dbPost.Visible)
                dbProposal.Visible = proposal.Visible;

            await _context.SaveChangesAsync();
            return new ServiceResponse<Proposal> { Data = proposal };
        }
        #endregion

        #region "Delete Methods"
        public async Task<ServiceResponse<bool>> DeleteProposal(int id)
        {
            bool isAdmin = (await _authService.IsAdmin()).Data;
            var proposal = await _context.Proposals.FirstOrDefaultAsync(p => p.Id == id && !p.Deleted && (p.Visible || isAdmin));
            if (proposal == null)
            {
                return new ServiceResponse<bool>
                {
                    Data = false,
                    Success = false,
                    Message = "Proposal not found to delete."
                };
            }

            proposal.Deleted = true;
            await _context.SaveChangesAsync();
            return new ServiceResponse<bool> { Data = true };
        }
        #endregion

        #region "Private Methods"
        private async Task<List<Proposal>> FindProposalsBySearchText(string searchText)
        {
            List<Proposal> result = new List<Proposal>();
            var proposals = await _context.Proposals.Where(p => !p.Deleted && p.Visible).ToListAsync();
            foreach (var proposal in proposals)
            {
                var teacher = await _context.Users.FindAsync(proposal.TeacherId);
                if (teacher.FirstName.ToLower().Contains(searchText.ToLower())
                    || teacher.LastName.ToLower().Contains(searchText.ToLower())
                    || (teacher.FirstName + " " + teacher.LastName).ToLower().Contains(searchText.ToLower()))
                {
                    result.Add(proposal);
                }
            }

            return result;
        } 
        #endregion
    }
}
