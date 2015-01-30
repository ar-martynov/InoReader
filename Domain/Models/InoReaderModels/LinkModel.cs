using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class LinkModel
    {
        public int LinkId { get; set; }
        public int? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
       
        public DateTime DateWhenAdded { get; set; }
        public List<TagModel> Tags { get; set; }

    }
}
