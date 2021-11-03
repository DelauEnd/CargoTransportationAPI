using Contracts;
using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Repository.Users
{
    public class RouteRepository : RepositoryBase<Route>, IRouteRepository
    {
        public RouteRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {

        }

        public Route GetRouteById(int Id, bool TrackChanges)
            => FindByCondition(Route => Route.Id == Id, TrackChanges)
            .Include(Route => Route.Cargoes).ThenInclude(Cargo => Cargo.Category)
            .Include(Route => Route.Transport)
            .SingleOrDefault();

        public IEnumerable<Route> GetAllRoutes(bool trackChanges)
            => FindAll(trackChanges)
            .Include(Route => Route.Transport)
            .ToList();

        public void CreateRoute(Route route)
            => Create(route);
    }
}
