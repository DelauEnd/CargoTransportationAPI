using Contracts;
using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Repository.Users
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {

        }

        public void CreateOrder(Order order)
            => Create(order);

        public IEnumerable<Order> GetAllOrders(bool trackChanges)
            => FindAll(trackChanges)
            .Include(route => route.Destination).Include(route => route.Sender)
            .ToList();


        public Order GetOrderById(int id, bool trackChanges)
            => FindByCondition(order => order.Id == id, trackChanges)
            .Include(order => order.Cargoes).ThenInclude(cargo => cargo.Category)
            .Include(route => route.Destination).Include(route => route.Sender)
            .SingleOrDefault();
    }
}
