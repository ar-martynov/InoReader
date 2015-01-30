using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class RssNew
    {
        public int RssNewId { get; set; }
        public int RssCanalId { get; set; }

        public string Title { get; set; }
        public string Url { get; set; }
        public DateTimeOffset PublishDate { get; set; }
        public string Text { get; set; }

        public RssCanal RssCanal { get; set; }
    }
}
