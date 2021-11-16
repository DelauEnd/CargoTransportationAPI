using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IOrderRepository
    {
        Task<Order> GetOrderByIdAsync(int Id, bool trackChanges);
        Task<IEnumerable<Order>> GetAllOrdersAsync(bool trackChanges);
        void CreateOrder(Order order);
        void DeleteOrder(Order order);
    }
}
