using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public interface IRepositoryManager
    {
        ICargoCategoryRepository CargoCategories { get; }
        ICargoRepository Cargoes { get; }
        ICustomerRepository Customers { get; }
        IOrderRepository Orders { get; }
        IRouteRepository Routes { get; }
        ITransportRepository Transports { get; }
        void Save();
    }
}
