using System.Collections.Generic;

namespace Domain.Models
{
    public class AllLinksAndCategories
    {
        public List<CategoryModel> CategoriesWithLinks { get; set; }
        public CategoryModel LinksWithoutCategory { get; set; }
    }
}
