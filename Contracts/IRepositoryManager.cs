using System.Threading.Tasks;

namespace Interfaces
{
    public interface IRepositoryManager
    {
        ICargoCategoryRepository CargoCategories { get; }
        ICargoRepository Cargoes { get; }
        ICustomerRepository Customers { get; }
        IOrderRepository Orders { get; }
        IRouteRepository Routes { get; }
        ITransportRepository Transport { get; }
        Task SaveAsync();
    }
}
