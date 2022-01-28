using Entities.Models;
using Entities.RequestFeautures;
using System.Threading.Tasks;

namespace Contracts
{
    public interface ICargoRepository
    {
        Task<PagedList<Cargo>> GetCargoesByOrderIdAsync(int id, CargoParameters parameters, bool trackChanges);
        Task<PagedList<Cargo>> GetCargoesByRouteIdAsync(int id, CargoParameters parameters, bool trackChanges);
        void CreateCargoForOrder(Cargo cargo, int orderId);
        Task AssignCargoToRoute(int cargoIds, int routeId);
        Task<Cargo> GetCargoByIdAsync(int cargoId, bool trackChanges);
        void DeleteCargo(Cargo cargo);
        Task<PagedList<Cargo>> GetAllCargoesAsync(CargoParameters parameters, bool trackChanges);
        Task<PagedList<Cargo>> GetUnassignedCargoesAsync(CargoParameters parameters, bool trackChanges);
    }
}
