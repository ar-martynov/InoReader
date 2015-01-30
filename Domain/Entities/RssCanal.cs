using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class RssCanal
    {
        public int RssCanalId { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }

        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<RssNew> RssNews { get; set; }
    }
}