using Contracts;
using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository.Users
{
    public class RouteRepository : RepositoryBase<Route>, IRouteRepository
    {
        public RouteRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {

        }

        public Route GetRouteById(int id, bool trackChanges)
            => FindByCondition(route => route.Id == id, trackChanges)
            .Include(route => route.Cargoes).ThenInclude(cargo => cargo.Category)
            .Include(route => route.Transport)
            .SingleOrDefault();

        public IEnumerable<Route> GetAllRoutes(bool trackChanges)
            => FindAll(trackChanges)
            .Include(route => route.Transport)
            .ToList();

        public void CreateRoute(Route route)
            => Create(route);

        public void DeleteRoute(Route route)
            => Delete(route);
    }
}
