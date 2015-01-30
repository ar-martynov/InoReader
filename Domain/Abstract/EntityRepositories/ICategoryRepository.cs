using System.Collections.Generic;
using Domain.Entities;

namespace Domain.Abstract
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {

        IEnumerable<Category> GetCategories(int userId);
        string GetCategoryName(int? categoryId, int userId);
        bool DeleteCategory(int categoryId, int userId);
    }
}
