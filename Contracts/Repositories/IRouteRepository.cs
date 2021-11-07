using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public interface IRouteRepository
    {
        Route GetRouteById(int Id, bool trackChanges);
        IEnumerable<Route> GetAllRoutes(bool trackChanges);
        void CreateRoute(Route route);
        void DeleteRoute(Route route);
    }
}
