using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface ICargoCategoryRepository
    {
        Task<CargoCategory> GetCategoryByCargoIdAsync(int id, bool trackChanges);
        Task<IEnumerable<CargoCategory>> GetAllCategoriesAsync(bool trackChanges);
        void CreateCategory(CargoCategory category);
        Task<CargoCategory> GetCategoryByIdAsync(int id, bool trackChanges);
        void DeleteCategory(CargoCategory category);
    }
}
