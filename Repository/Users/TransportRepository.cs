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

        public IEnumerable<Transport> GetAllTransport(bool TrackChanges)
            => FindAll(TrackChanges)
            .ToList();

        public Transport GetTransportById(int Id, bool TrackChanges)
            => FindByCondition(Transport => Transport.Id == Id, TrackChanges)
            .SingleOrDefault();
    }
}
