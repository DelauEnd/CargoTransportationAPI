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

        public void CreateCategory(CargoCategory category)
            => Create(category);

        public void DeleteCategory(CargoCategory category)
            => Delete(category);

        public IEnumerable<CargoCategory> GetAllCategories(bool trackChanges)
            => FindAll(trackChanges)
            .ToList();

        public CargoCategory GetCategoryByCargoId(int id, bool trackChanges)
            => FindByCondition(category => 
            category.Cargoes.Where(cargo => cargo.Id == id).Any(), trackChanges)
            .SingleOrDefault();

        public CargoCategory GetCategoryById(int id, bool trackChanges)
            => FindByCondition(category =>
            category.Id == id, trackChanges)
            .SingleOrDefault();
    }
}
