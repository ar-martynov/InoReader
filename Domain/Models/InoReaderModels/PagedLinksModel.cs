using System;
using System.Collections.Generic;


namespace Domain.Models
{
    public class PagedLinksModel
    {
        public List<LinkModel> ReaderLinks { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string ActionName { get; set; }
        public dynamic ItemId { get; set; }
        public bool Order { get; set; }

        public bool? FirstRequest { get; set; }

    }
}
