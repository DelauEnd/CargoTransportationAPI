using Contracts;
using Entities;
using Entities.Models;

namespace Repository.Users
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {

        }
    }
}
