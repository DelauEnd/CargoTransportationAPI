using Entities.DataTransferObjects;
using Entities.Models;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IAuthenticationManager
    {
        Task<User> ReturnUserIfValid(UserForAuthenticationDto userForAuthentication);
        Task<string> CreateToken(User user);
    }
}
