using System.Collections.Generic;
using System.Linq;
using System;

using Domain.Abstract;
using Domain.Entities;

namespace Domain.Concrete
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(IMainContext context)
            : base(context) { }

        
        public bool DeleteCategory(int categoryId, int userId)
        {
            try
            {
                var categoryToDelete = _dbSet.FirstOrDefault(x => x.CategoryId == categoryId && x.UserId == userId);

                foreach (var link in categoryToDelete.Links)
                {
                    link.CategoryId = null;
                }
                _dbSet.Remove(categoryToDelete);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<Category> GetCategories(int userId)
        {
            return _dbSet.Where(x => x.UserId == userId).OrderBy(x => x.Title);
        }

        public string GetCategoryName(int? categoryId, int userId)
        {
            if (categoryId == null) return "Без категории";

            return _dbSet.First(x => x.UserId == userId && x.CategoryId == categoryId).Title;
        }
    }
}
