using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace net7.Repositories.Authentication
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        public Task<ServiceResponse<string>> Login(string userName, string password)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<int>> Register(User user, string password)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<bool>> UserExist(string userName)
        {
            throw new NotImplementedException();
        }
    }
}