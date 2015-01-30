using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        //навигационные свойства 
        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<RssCanal> RssCanals { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ICollection<Link> Links { get; set; }
        

    }
}