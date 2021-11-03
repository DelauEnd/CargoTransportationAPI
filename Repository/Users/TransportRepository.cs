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

        public IEnumerable<Transport> GetAllTransport(bool TrackChanges)
            => FindAll(TrackChanges)
            .ToList();

        public Transport GetTransportById(int id, bool TrackChanges)
            => FindByCondition(Transport => Transport.Id == id, TrackChanges)
            .SingleOrDefault();

        public Transport GetTransportByRegistrationNumber(string number, bool TrackChanges)
        => FindByCondition(Transport => Transport.RegistrationNumber == number, TrackChanges)
            .SingleOrDefault();
    }
}
