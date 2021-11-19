using Entities.Models;
using Entities.RequestFeautures;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts
{
    public interface ICargoRepository
    {
        Task<PagedList<Cargo>> GetCargoesByOrderIdAsync(int id, RequestParameters parameters, bool trackChanges);
        Task<PagedList<Cargo>> GetCargoesByRouteIdAsync(int id, RequestParameters parameters, bool trackChanges);
        void CreateCargoForOrder(Cargo cargo, int OrderId);
        Task MarkTheCargoToRouteAsync(int cargoId, int routeId);
        Task<Cargo> GetCargoByIdAsync(int cargoId, bool trackChanges);
        void DeleteCargo(Cargo cargo);
        Task<PagedList<Cargo>> GetAllCargoesAsync(RequestParameters parameters, bool trackChanges);
    }
}
