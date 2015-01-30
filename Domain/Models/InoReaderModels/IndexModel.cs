using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class IndexModel
    {
        public bool IsLinksEnabled { get; set; }
        public bool IsRssNewsEnabled { get; set; }
        public PagedLinksModel PagedLinks { get; set; }
        public List<RssNewsItemModel> News { get; set; }
        public int NewsTotalPages { get; set; }
        public int NewsCurrentPage { get; set; }
    }
}
