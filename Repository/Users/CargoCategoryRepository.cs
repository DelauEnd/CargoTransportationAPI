using Contracts;
using Entities;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository.Users
{
    public class CargoCategoryRepository : RepositoryBase<CargoCategory>, ICargoCategoryRepository
    {
        public CargoCategoryRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {

        }

        public CargoCategory GetCategoryByCargoId(int id, bool trackChanges)
            => FindByCondition(category => 
            category.Cargoes.Where(cargo => cargo.Id == id).Any(), trackChanges)
            .SingleOrDefault();
    }
}
