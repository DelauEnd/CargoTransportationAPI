using Contracts;
using Entities;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Users
{
    public class CargoCategoryRepository : RepositoryBase<CargoCategory>, ICargoCategoryRepository
    {
        public CargoCategoryRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {

        }
    }
}
