using Contracts;
using Entities;
using Entities.DataTransferObjects;
using Entities.Models;
using System.Collections.Generic;
using System.Linq;

namespace Repository.Users
{
    public class TransportRepository : RepositoryBase<Transport>, ITransportRepository
    {
        public TransportRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public void CreateTransport(Transport transport)
            => Create(transport);

        public void DeleteTransport(Transport transport)
            => Delete(transport);

        public IEnumerable<Transport> GetAllTransport(bool trackChanges)
            => FindAll(trackChanges)
            .ToList();

        public Transport GetTransportById(int id, bool trackChanges)
            => FindByCondition(transport => transport.Id == id, trackChanges)
            .SingleOrDefault();

        public Transport GetTransportByRegistrationNumber(string number, bool trackChanges)
        => FindByCondition(transport => transport.RegistrationNumber == number, trackChanges)
            .SingleOrDefault();
    }
}
