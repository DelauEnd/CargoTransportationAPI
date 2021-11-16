using Contracts;
using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<IEnumerable<CargoCategory>> GetAllCategoriesAsync(bool trackChanges)
            => await FindAll(trackChanges)
            .ToListAsync();

        public async Task<CargoCategory> GetCategoryByCargoIdAsync(int id, bool trackChanges)
            => await FindByCondition(category =>
             category.Cargoes.Where(cargo => cargo.Id == id).Any(), trackChanges)
            .SingleOrDefaultAsync();

        public async Task<CargoCategory> GetCategoryByIdAsync(int id, bool trackChanges)
            => await FindByCondition(category =>
            category.Id == id, trackChanges)
            .SingleOrDefaultAsync();
    }
}
