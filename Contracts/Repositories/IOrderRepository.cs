using Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> GetOrderByIdAsync(int Id, bool trackChanges);
        Task<IEnumerable<Order>> GetAllOrdersAsync(bool trackChanges);
        void CreateOrder(Order order);
        void DeleteOrder(Order order);
    }
}
