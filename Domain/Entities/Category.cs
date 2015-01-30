using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Category
    {
        public int CategoryId { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }

        //навигационные свойства 
        public virtual User User { get; set; }
        public virtual ICollection<Link> Links { get; set; }
        

     
    }
}
