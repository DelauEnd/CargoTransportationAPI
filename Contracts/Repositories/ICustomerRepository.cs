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
    }
}
