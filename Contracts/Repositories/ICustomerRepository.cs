using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public interface ICustomerRepository
    {
        Customer GetSenderByOrderId(int id, bool trackChanges);
        Customer GetDestinationByOrderId(int id, bool trackChanges);
        IEnumerable<Customer> GetAllCustomers(bool trackChangess);
        Customer GetCustomerById(int id, bool trackChanges);
        void CreateCustomer(Customer customer);
        void DeleteCustomer(Customer customer);
    }
}
