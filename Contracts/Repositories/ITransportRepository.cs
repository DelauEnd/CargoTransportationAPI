using Entities.DataTransferObjects;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public interface ITransportRepository
    {
        IEnumerable<Transport> GetAllTransport(bool trackChanges);
        Transport GetTransportById(int id, bool trackChanges);
        Transport GetTransportByRegistrationNumber(string number, bool trackChanges);
        void CreateTransport(Transport transport);
    }
}
