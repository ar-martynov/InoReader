using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Link
    {
        public int LinkId { get; set; }

        public int UserId { get; set; }
        public int? CategoryId { get; set; }

        public string Title { get; set; }
        public string Url { get; set; }
        
        public DateTime DateWhenAdded { get; set; }

        //навигационные свойства 
        public virtual User User { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
       
    }
}
