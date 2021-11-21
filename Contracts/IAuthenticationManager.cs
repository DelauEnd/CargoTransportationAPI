using Entities.DataTransferObjects;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IAuthenticationManager
    {
        Task<User> ReturnUserIfValid(UserForAuthenticationDto userForAuthentication); 
        Task<string> CreateToken(User user);
    }
}
