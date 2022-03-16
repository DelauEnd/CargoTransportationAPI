using DTO.ResponseDTO;
using Entities.Models;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IAuthenticationManager
    {
        Task<User> ReturnUserIfValid(UserForAuthenticationDto userForAuthentication);
        Task<string> CreateToken(User user);
    }
}
