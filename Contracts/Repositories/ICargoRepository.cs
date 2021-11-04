using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public interface ICargoRepository
    {
        IEnumerable<Cargo> GetCargoesByOrderId(int id, bool trackChanges);
        void CreateCargoForOrder(Cargo cargo, int OrderId);
    }
}
