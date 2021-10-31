using Entities.DataTransferObjects;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public interface ITransportRepository
    {
        IEnumerable<Transport> GetAllTransport(bool TrackChanges);
        Transport GetTransportById(int Id, bool TrackChanges);
    }
}
