using Contracts;
using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        {
            Delete(cargo);
        }

        public IEnumerable<Cargo> GetAllCargoes(bool trackChanges)
         => FindAll(trackChanges).Include(cargo => cargo.Category).ToList();

        public Cargo GetCargoById(int id, bool trackChanges)
            => FindByCondition(cargo => cargo.Id == id, trackChanges)
            .Include(cargo => cargo.Category)
            .SingleOrDefault();

        public IEnumerable<Cargo> GetCargoesByOrderId(int id, bool trackChanges)
            => FindByCondition(cargo => cargo.OrderId == id, trackChanges)
            .Include(cargo => cargo.Category)
            .ToList();

        public IEnumerable<Cargo> GetCargoesByRouteId(int id, bool trackChanges)
            => FindByCondition(cargo => cargo.RouteId == id, trackChanges)
            .Include(cargo => cargo.Category)
            .ToList();

        public void MarkTheCargoToRoute(int cargoId, int routeId)
        {
            var route = FindByCondition(cargo => cargo.Id == cargoId, false).FirstOrDefault();
            route.RouteId = routeId;
            Update(route);
        }

        public void UnmarkTheCargoFromRoute(int id)
        {
            var route = FindByCondition(cargo => cargo.Id == id, false).FirstOrDefault();
            route.RouteId = null;
            Update(route);
        }
    }
}
