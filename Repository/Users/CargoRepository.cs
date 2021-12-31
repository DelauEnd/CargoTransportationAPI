using Contracts;
using Entities;
using Entities.Models;
using Entities.RequestFeautures;
using Microsoft.EntityFrameworkCore;
using Repository.Extensions;
using System.Linq;
using System.Threading.Tasks;

namespace Repository.Users
{
    public class CargoRepository : RepositoryBase<Cargo>, ICargoRepository
    {
        public CargoRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {

        }

        public void CreateCargoForOrder(Cargo cargo, int OrderId)
        {
            cargo.OrderId = OrderId;
            Create(cargo);
        }

        public void DeleteCargo(Cargo cargo)
            => Delete(cargo);
        

        public async Task<PagedList<Cargo>> GetAllCargoesAsync(CargoParameters parameters, bool trackChanges)
        {
            var cargoes = await FindAll(trackChanges)
                .Include(cargo => cargo.Category)
                .ApplyFilters(parameters)
                .Search(parameters.Search)
                .Sort(parameters)
                .ToListAsync();

            var cargoPagedList = cargoes.ToPagedList(parameters.Page, parameters.PageSize);
            return cargoPagedList;
        }

        public async Task<Cargo> GetCargoByIdAsync(int id, bool trackChanges)
            => await FindByCondition(cargo => cargo.Id == id, trackChanges)
                .Include(cargo => cargo.Category)
                .SingleOrDefaultAsync();


        public async Task<PagedList<Cargo>> GetCargoesByOrderIdAsync(int id, CargoParameters parameters, bool trackChanges)
        {
            var cargoes = await FindByCondition(cargo =>
            cargo.OrderId == id, trackChanges)
                .Include(cargo => cargo.Category)
                .ApplyFilters(parameters)
                .Search(parameters.Search)
                .Sort(parameters)
                .ToListAsync();

            var cargoPagedList = cargoes.ToPagedList(parameters.Page, parameters.PageSize);
            return cargoPagedList;
        }

        public async Task<PagedList<Cargo>> GetCargoesByRouteIdAsync(int id, CargoParameters parameters, bool trackChanges)
        {
            var cargoes = await FindByCondition(cargo =>
            cargo.RouteId == id, trackChanges)
                .Include(cargo => cargo.Category)
                .ApplyFilters(parameters)
                .Search(parameters.Search)
                .Sort(parameters)
                .ToListAsync();

            var cargoPagedList = cargoes.ToPagedList(parameters.Page, parameters.PageSize);
            return cargoPagedList;
        }

        public async Task AssignCargoToRoute(int cargoId, int routeId)
            => await ExecQuery($"exec AssignCargoToRoute @cargoId={cargoId}, @routeId={routeId}");

        public async Task<PagedList<Cargo>> GetUnassignedCargoesAsync(CargoParameters parameters, bool trackChanges)
        {
            var cargoes = await FindAll(trackChanges)
                .Include(cargo => cargo.Category)
                .Where(cargo=>cargo.RouteId == null)
                .ApplyFilters(parameters)
                .Search(parameters.Search)
                .Sort(parameters)
                .ToListAsync();

            var cargoPagedList = cargoes.ToPagedList(parameters.Page, parameters.PageSize);
            return cargoPagedList;
        }
    }
}
