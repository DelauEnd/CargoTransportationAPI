using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public interface IRouteRepository
    {
        public Route GetRouteById(int Id, bool TrackChanges);

        IEnumerable<Route> GetAllRoutes(bool trackChanges);
    }
}
