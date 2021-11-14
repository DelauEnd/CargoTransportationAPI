using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public interface IOrderRepository
    {
        Order GetOrderById(int Id, bool trackChanges);
        IEnumerable<Order> GetAllOrders(bool trackChanges);
        void CreateOrder(Order order);
        void DeleteOrder(Order order);
    }
}
