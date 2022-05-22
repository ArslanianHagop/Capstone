using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Capstone1.Server.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(DataContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }
        #region "Authentication Methods"
        #region "Get Methods"
        public async Task<bool> UserExists(string email)
        {
            if (await _context.Users.AnyAsync(user => !user.Deleted && user.Visible && user.Email.ToLower().Equals(email.ToLower())))
            {
                return true;
            }
            return false;
        }

        public async Task<bool> UserExists(string firstName, string lastName)
        {
            if (await _context.Users.AnyAsync(user => !user.Deleted && user.Visible && user.FirstName.ToLower().Equals(firstName.ToLower())
                 && user.LastName.ToLower().Equals(lastName.ToLower())))
            {
                return true;
            }
            return false;
        }
        #endregion

        #region "Post Methods"
        public async Task<ServiceResponse<int>> Register(User user, string password)
        {
            if (await UserExists(user.Email))
            {
                return new ServiceResponse<int>
                {
                    Success = false,
                    Message = "User already exists."
                };
            }

            if (await UserExists(user.FirstName, user.LastName))
            {
                return new ServiceResponse<int>
                {
                    Success = false,
                    Message = "User with this exact first name and last name already exists."
                };
            }

            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();


            if ((user.Role).Equals("Student"))
            {
                _context.Students.Add(new Student { StudentId = user.Id });
            }
            else if ((user.Role).Equals("Teacher"))
            {
                _context.Teachers.Add(new Teacher { TeacherId = user.Id });
            }

            await _context.SaveChangesAsync();

            return new ServiceResponse<int> { Data = user.Id, Message = "Registration successful!" };
        }

        public async Task<ServiceResponse<string>> Login(string email, string password)
        {
            var response = new ServiceResponse<string>();
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower().Equals(email.ToLower()) && !u.Deleted && (u.Visible || u.Role.Equals("Admin")));
            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found.";
            }
            else if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                response.Success = false;
                response.Message = "Wrong password.";
            }
            else
            {
                response.Data = CreateToken(user);
            }

            return response;
        }

        public async Task<ServiceResponse<bool>> ChangePassword(int userId, string newPassword)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return new ServiceResponse<bool>
                {
                    Success = false,
                    Message = "User not found."
                };
            }

            CreatePasswordHash(newPassword, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _context.SaveChangesAsync();

            return new ServiceResponse<bool> { Data = true, Message = "Password has been changed." };
        }

        public async Task<ServiceResponse<bool>> SecurityPasswordChange(string currentPassword)
        {
            ServiceResponse<bool> response = new ServiceResponse<bool>();
            var user = await _context.Users.FindAsync(GetUserId());
            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found.";
            }
            else if (!VerifyPasswordHash(currentPassword, user.PasswordHash, user.PasswordSalt))
            {
                response.Success = false;
                response.Message = "Wrong password.";
            }
            else
            {
                response.Data = true;
            }

            return response;
        }
        #endregion

        #region "Private Hash Methods"
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
        #endregion

        #region "Private Create Token Method"
        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8
                .GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
        #endregion

        #endregion

        #region "User Methods"
        #region "Get Methods"
        public async Task<int> GetUserId()
        {
            if (await CheckUserExpiration())
            {
                return 0;
            }
            else
            {
                return int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            }
        }

        public async Task<bool> CheckUserExpiration()
        {
            if (_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) != null)
                return false;
            else
                return true;
        }

        public async Task<ServiceResponse<UserChange>> GetUserProfile()
        {
            return await GetUserProfileById(await GetUserId());
        }

        public async Task<ServiceResponse<UserChange>> GetUserProfileById(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return new ServiceResponse<UserChange>
                {
                    Success = false,
                    Message = "User not found."
                };
            }

            var userLanguages = await _context.UserLanguages.Where(ul => ul.UserId == user.Id).ToListAsync();
            return new ServiceResponse<UserChange>
            {
                Data = new UserChange
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    ProfilePic = user.ProfilePic,
                    Visible = user.Visible,
                    UserLanguages = userLanguages
                }
            };
        }

        public async Task<ServiceResponse<int>> GetCurrentUserId()
        {
            int userId = await GetUserId();
            if (userId == 0)
            {
                return new ServiceResponse<int>
                {
                    Success = false,
                    Message = "The current user is not logged in."
                };
            }

            return new ServiceResponse<int> { Data = userId };
        }

        public async Task<ServiceResponse<UserDTO>> GetUsersPaginated(int page)
        {
            var pageResults = 5f;
            double pageCount = 0;
            int? currentUserId = await GetUserId();
            List<User> result = new List<User>();
            if(currentUserId == 0)
            {
                pageCount = Math.Ceiling((double)await _context.Users.Where(u => !u.Deleted && u.Visible).CountAsync() / pageResults);
                result = await _context.Users.Where(u => !u.Deleted && u.Visible).Skip((page - 1) * (int)pageResults).Take((int)pageResults).ToListAsync();
            }
            else
            {
                pageCount = Math.Ceiling((double)await _context.Users.Where(u => !u.Deleted && u.Visible && u.Id != currentUserId).CountAsync() / pageResults);
                result = await _context.Users.Where(u => !u.Deleted && u.Visible && u.Id != currentUserId).Skip((page - 1) * (int)pageResults).Take((int)pageResults).ToListAsync();
            }
            var response = new ServiceResponse<UserDTO>
            {
                Data = new UserDTO
                {
                    Users = result,
                    CurrentPage = page,
                    Pages = (int)pageCount
                }
            };

            return response;
        }

        public async Task<ServiceResponse<List<UserLanguage>>> GetAllUserLanguages()
        {
            List<UserLanguage> userLanguages = await _context.UserLanguages.ToListAsync();
            if (userLanguages == null || userLanguages.Count == 0)
            {
                return new ServiceResponse<List<UserLanguage>>
                {
                    Success = false,
                    Message = "No user has selected any languages."
                };
            }

            return new ServiceResponse<List<UserLanguage>> { Data = userLanguages };
        }

        public async Task<ServiceResponse<UserDTO>> SearchUsers(string searchText, int page)
        {
            var pageResults = 5f;
            var pageCount = Math.Ceiling((await FindUsersBySearchText(searchText)).Count() / pageResults);
            var users = await FindUsersBySearchTextPagination(searchText, page);

            var response = new ServiceResponse<UserDTO>
            {
                Data = new UserDTO
                {
                    Users = users,
                    CurrentPage = page,
                    Pages = (int)pageCount
                }
            };

            return response;
        }

        private async Task<List<User>> FindUsersBySearchTextPagination(string searchText, int page)
        {
            var pageResults = 5f;
            var users = await FindUsersBySearchText(searchText);
            var usersPaginated = users.Skip((page - 1) * (int)pageResults).Take((int)pageResults).ToList();

            return usersPaginated;
        }

        private async Task<List<User>> FindUsersBySearchText(string searchText)
        {
            int? currentUserId = await GetUserId();
            List<User> users = new List<User>();
            if(currentUserId == 0)
            {
                users = await _context.Users
                    .Where(u => (!u.Deleted && u.Visible)
                    && (u.FirstName.ToLower().Contains(searchText.ToLower())
                    || u.LastName.ToLower().Contains(searchText.ToLower())
                    || (u.FirstName + " " + u.LastName).ToLower().Contains(searchText.ToLower()))).ToListAsync();
            }
            else
            {
                users = await _context.Users
                    .Where(u => (!u.Deleted && u.Visible && u.Id != currentUserId)
                    && (u.FirstName.ToLower().Contains(searchText.ToLower())
                    || u.LastName.ToLower().Contains(searchText.ToLower())
                    || (u.FirstName + " " + u.LastName).ToLower().Contains(searchText.ToLower()))).ToListAsync();
            }

            return users;
        }

        public async Task<ServiceResponse<List<string>>> GetUserSearchSuggestions(string searchText)
        {
            var users = await FindUsersBySearchText(searchText);

            List<string> result = new List<string>();

            foreach (var user in users)
            {
                result.Add(user.FirstName + " " + user.LastName);
            }

            return new ServiceResponse<List<string>> { Data = result };
        }

        public async Task<ServiceResponse<UserDTO>> GetAdminUsersPaginated(int page)
        {
            var pageResults = 5f;
            var pageCount = Math.Ceiling((double)await _context.Users.Where(u => !u.Deleted).CountAsync() / pageResults);
            var result = await _context.Users.Where(u => !u.Deleted).Skip((page - 1) * (int)pageResults).Take((int)pageResults).ToListAsync();
            var response = new ServiceResponse<UserDTO>
            {
                Data = new UserDTO
                {
                    Users = result,
                    CurrentPage = page,
                    Pages = (int)pageCount
                }
            };

            return response;
        }

        public async Task<ServiceResponse<UserDTO>> SearchAdminUsers(string searchText, int page)
        {
            var pageResults = 5f;
            var pageCount = Math.Ceiling((await FindAdminUsersBySearchText(searchText)).Count() / pageResults);
            var users = await FindAdminUsersBySearchTextPagination(searchText, page);

            var response = new ServiceResponse<UserDTO>
            {
                Data = new UserDTO
                {
                    Users = users,
                    CurrentPage = page,
                    Pages = (int)pageCount
                }
            };

            return response;
        }

        public async Task<ServiceResponse<List<string>>> GetAdminUserSearchSuggestions(string searchText)
        {
            var users = await FindAdminUsersBySearchText(searchText);

            List<string> result = new List<string>();

            foreach (var user in users)
            {
                result.Add(user.FirstName + " " + user.LastName);
            }

            return new ServiceResponse<List<string>> { Data = result };
        }

        public async Task<ServiceResponse<int>> GetFirstAdminId()
        {
            var adminUser = await _context.Users.FirstOrDefaultAsync(u => !u.Deleted && u.Role.Equals("Admin"));
            if (adminUser == null)
            {
                return new ServiceResponse<int>
                {
                    Success = false,
                    Message = "No admin user was found."
                };
            }

            return new ServiceResponse<int> { Data = adminUser.Id };
        }

        public async Task<ServiceResponse<string>> GetUserFullName(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if(user == null)
            {
                return new ServiceResponse<string>
                {
                    Success = false,
                    Message = "user not found to get its full name."
                };
            }

            string fullName = user.FirstName + " " + user.LastName;

            return new ServiceResponse<string> { Data = fullName };
        }

        private async Task<List<User>> FindAdminUsersBySearchTextPagination(string searchText, int page)
        {
            var pageResults = 5f;
            var users = await FindAdminUsersBySearchText(searchText);
            var usersPaginated = users.Skip((page - 1) * (int)pageResults).Take((int)pageResults).ToList();

            return usersPaginated;
        }

        private async Task<List<User>> FindAdminUsersBySearchText(string searchText)
        {
            var users = await _context.Users
                .Where(u => !u.Deleted
                && (u.FirstName.ToLower().Contains(searchText.ToLower())
                || u.LastName.ToLower().Contains(searchText.ToLower())
                || (u.FirstName + " " + u.LastName).ToLower().Contains(searchText.ToLower()))).ToListAsync();

            return users;
        }

        private async Task<User> GetUserById(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<ServiceResponse<bool>> IsAdmin()
        {
            if (!_httpContextAccessor.HttpContext.User.IsInRole("Admin"))
            {
                return new ServiceResponse<bool>
                {
                    Success = false,
                    Message = "You're not a teacher."
                };
            }

            return new ServiceResponse<bool> { Data = true, Message = "You are an admin." };
        }
        #endregion

        #region "Post Methods"
        public async Task<ServiceResponse<bool>> ChangeUserProfile(string firstName, string lastName, string ProfilePic, bool visible, List<UserLanguage> userLanguages)
        {
            return await ChangeUserProfileById(await GetUserId(), firstName, lastName, ProfilePic, visible, userLanguages);
        }

        public async Task<ServiceResponse<bool>> ChangeUserProfileById(int id, string firstName, string lastName, string ProfilePic, bool visible, List<UserLanguage> userLanguages)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return new ServiceResponse<bool>
                {
                    Success = false,
                    Message = "User not found."
                };
            }

            user.FirstName = firstName;
            user.LastName = lastName;
            user.ProfilePic = ProfilePic;
            if (user.Visible.CompareTo(visible) != 0)
            {
                user.Visible = visible;
                if (user.Role.Equals("Student"))
                {
                    var posts = await _context.Posts.Where(p => p.StudentId == user.Id && !p.Deleted).ToListAsync();
                    foreach (var post in posts)
                    {
                        var dbPost = await _context.Posts.FirstOrDefaultAsync(p => p.Id == post.Id);
                        if (dbPost != null)
                        {
                            dbPost.Visible = visible;
                            var proposals = await _context.Proposals.Where(p => p.PostId == dbPost.Id && !p.Deleted).ToListAsync();
                            foreach (var proposal in proposals)
                            {
                                var dbTeacher = await _context.Users.FirstOrDefaultAsync(u => u.Id == proposal.TeacherId);
                                if (dbTeacher != null && dbTeacher.Visible && (visible ? dbPost.Visible : !dbPost.Visible))
                                {
                                    var dbProposal = await _context.Proposals.FirstOrDefaultAsync(p => p.Id == proposal.Id);
                                    if (dbProposal != null)
                                        dbProposal.Visible = visible;
                                }
                            }
                            await _context.SaveChangesAsync();
                        }
                    }
                }
                else if (user.Role.Equals("Teacher"))
                {
                    var proposals = await _context.Proposals.Where(p => p.TeacherId == user.Id && !p.Deleted).ToListAsync();
                    foreach (var proposal in proposals)
                    {
                        var dbPost = await _context.Posts.FirstOrDefaultAsync(p => p.Id == proposal.PostId);
                        if (dbPost.Visible)
                        {
                            var dbProposal = await _context.Proposals.FirstOrDefaultAsync(p => p.Id == proposal.Id);
                            if (dbProposal != null)
                                dbProposal.Visible = visible;
                        }
                    }
                }

                var chats = await _context.Chats.Where(c => (c.SenderId == user.Id || c.RecipientId == user.Id) && !c.Deleted).ToListAsync();
                foreach (var chat in chats)
                {
                    var dbChat = await _context.Chats.FirstOrDefaultAsync(c => c.Id == chat.Id);
                    if (dbChat != null)
                        dbChat.Visible = visible;
                }
            }

			for (int i = 0; i < userLanguages.Count; i++)
			{
				for (int j = i + 1; j < userLanguages.Count; j++)
				{
                    if(userLanguages[i].LanguageId == userLanguages[j].LanguageId)
					{
                        return new ServiceResponse<bool>
                        {
                            Data = false,
                            Success = false,
                            Message = "You have selected the same language more than once."
                        };
					}
				}
			}

            var dbLanguages = await _context.UserLanguages.Where(ul => ul.UserId == user.Id).ToListAsync();
            foreach (var userLanguage in dbLanguages)
            {
                _context.UserLanguages.Remove(userLanguage);
            }

            await _context.SaveChangesAsync();

            foreach (var userLanguage in userLanguages)
            {
                userLanguage.UserId = user.Id;
            }

            user.UserLanguages = userLanguages;

            await _context.SaveChangesAsync();

            return new ServiceResponse<bool> { Data = true, Message = "Changes made Successfully." };
        }
        #endregion

        #region "Delete Methods"
        public async Task<ServiceResponse<List<User>>> DeleteUser(int id, int page)
        {
            User user = await GetUserById(id);
            if (user == null)
            {
                return new ServiceResponse<List<User>>
                {
                    Success = false,
                    Message = "This user doesn't exist."
                };
            }

            user.Deleted = true;

            if (user.Role.Equals("Student"))
            {
                var posts = await _context.Posts.Where(p => p.StudentId == user.Id && !p.Deleted).ToListAsync();
                foreach (var post in posts)
                {
                    var dbPost = await _context.Posts.FirstOrDefaultAsync(p => p.Id == post.Id);
                    if (dbPost != null)
                    {
                        dbPost.Deleted = true;
                        var proposals = await _context.Proposals.Where(p => p.PostId == dbPost.Id && !p.Deleted).ToListAsync();
                        foreach (var proposal in proposals)
                        {
                            var dbProposal = await _context.Proposals.FirstOrDefaultAsync(p => p.Id == proposal.Id);
                            if (dbProposal != null)
                                dbProposal.Deleted = true;
                        }
                        await _context.SaveChangesAsync();
                    }
                }
            }
            else if (user.Role.Equals("Teacher"))
            {
                var proposals = await _context.Proposals.Where(p => p.TeacherId == user.Id && !p.Deleted).ToListAsync();
                foreach (var proposal in proposals)
                {
                    var dbProposal = await _context.Proposals.FirstOrDefaultAsync(p => p.Id == proposal.Id);
                    if (dbProposal != null)
                        dbProposal.Deleted = true;
                }
            }

            var chats = await _context.Chats.Where(c => (c.SenderId == user.Id || c.RecipientId == user.Id) && !c.Deleted).ToListAsync();
            foreach (var chat in chats)
            {
                var dbChat = await _context.Chats.FirstOrDefaultAsync(c => c.Id == chat.Id);
                if (dbChat != null)
                    dbChat.Deleted = true;
            }

            await _context.SaveChangesAsync();

            return new ServiceResponse<List<User>> { Data = (await GetAdminUsersPaginated(page)).Data.Users };
        }
        #endregion
        #endregion
    }
}
