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

        public IEnumerable<Cargo> GetCargoesByOrderId(int id, bool trackChanges)
            => FindByCondition(cargo => cargo.OrderId == id, trackChanges)
            .Include(cargo => cargo.Category)
            .ToList();
    }
}
