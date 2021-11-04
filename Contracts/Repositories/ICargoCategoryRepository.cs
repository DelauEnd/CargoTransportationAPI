using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public interface ICargoCategoryRepository
    {
        CargoCategory GetCategoryByCargoId(int id, bool trackChanges);
    }
}
