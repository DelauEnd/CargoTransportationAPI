using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public interface ICargoCategoryRepository
    {
        CargoCategory GetCategoryByCargoId(int id, bool trackChanges);
        IEnumerable<CargoCategory> GetAllCategories(bool trackChanges);
        void CreateCategory(CargoCategory category);
        CargoCategory GetCategoryById(int id, bool trackChanges);
        void DeleteCategory(CargoCategory category);
    }
}
