using Contracts;
using Entities;
using Entities.Models;
using System.Collections.Generic;
using System.Linq;

namespace Repository.Users
{
    public class CustomerRepository : RepositoryBase<Customer>, ICustomerRepository
    {
        public CustomerRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {

        }

        public Customer GetDestinationByOrderId(int id, bool trackChanges)
            => FindByCondition(destination => 
            destination.OrderDestination.Where(order => order.Id == id).Any() , trackChanges)
            .SingleOrDefault();

        public Customer GetSenderByOrderId(int id, bool trackChanges)
            => FindByCondition(sender =>
            sender.OrderSender.Where(order => order.Id == id).Any(), trackChanges)
            .SingleOrDefault();
    }
}
