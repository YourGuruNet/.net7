using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net7.Repositories.Authentication
{
    public class AuthenticationRepository : IAuthenticationRepository
    {

        private readonly IMapper _mapper;
        private readonly DataContext _dataContext;

        public AuthenticationRepository(IMapper mapper, DataContext dataContext)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }
        public Task<ServiceResponse<string>> Login(string userName, string password)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse<int>> Register(User user, string password)
        {
            var response = new ServiceResponse<int>();
            if(await UserExist(user.UserName)) {
                response.Success = false;
                response.Message = "UserName already taken.";
                return response;
            }
            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            _dataContext.Users.Add(user);
            await _dataContext.SaveChangesAsync();
            response.Data = user.Id;
           
            return response;
        }

        public async Task<bool> UserExist(string userName)
        {
            var user = await _dataContext.Users.AnyAsync(user => user.UserName.ToLower() == userName.ToLower());
           return user;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt) 
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF32.GetBytes(password));
            }
        }
    }
}