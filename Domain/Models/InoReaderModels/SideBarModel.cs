using System;
using System.Collections.Generic;
using System.Linq;

using Domain.Entities;


namespace Domain.Models
{
    public class SideBarModel
    {
        public List<SimpleCategoryModel> CategoriesList { get; set; }
        public List<TagModel> TagsList { get; set; }
        public List<RssCanalModel> RssCanals { get; set; }

    }
}
