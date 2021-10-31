using Contracts;
using Entities;
using Entities.Models;

namespace Repository.Users
{
    public class CustomerRepository : RepositoryBase<Customer>, ICustomerRepository
    {
        public CustomerRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {

        }
    }
}
