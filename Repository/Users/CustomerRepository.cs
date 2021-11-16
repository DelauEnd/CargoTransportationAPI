using Contracts;
using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository.Users
{
    public class CustomerRepository : RepositoryBase<Customer>, ICustomerRepository
    {
        public CustomerRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {

        }

        public void CreateCustomer(Customer customer)
            => Create(customer);

        public void DeleteCustomer(Customer customer)
            => Delete(customer);

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync(bool trackChangess)
            => await FindAll(trackChangess)
            .ToListAsync();

        public async Task<Customer> GetCustomerByIdAsync(int id, bool trackChanges)
            => await FindByCondition(destination =>
            destination.Id == id, trackChanges)
            .SingleOrDefaultAsync();

        public async Task<Customer> GetDestinationByOrderIdAsync(int id, bool trackChanges)
            => await FindByCondition(destination => 
            destination.OrderDestination.Where(order => order.Id == id).Any() , trackChanges)
            .SingleOrDefaultAsync();

        public async Task<Customer> GetSenderByOrderIdAsync(int id, bool trackChanges)
            => await FindByCondition(sender =>
            sender.OrderSender.Where(order => order.Id == id).Any(), trackChanges)
            .SingleOrDefaultAsync();
    }
}
