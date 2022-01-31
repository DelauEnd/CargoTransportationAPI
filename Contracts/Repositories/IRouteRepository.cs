using Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IRouteRepository
    {
        Task<Route> GetRouteByIdAsync(int id, bool trackChanges);
        Task<IEnumerable<Route>> GetAllRoutesAsync(bool trackChanges);
        void CreateRoute(Route route);
        void DeleteRoute(Route route);
    }
}
