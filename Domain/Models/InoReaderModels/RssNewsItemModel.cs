using System;
using System.Collections.Generic;


namespace Domain.Models
{
    public class RssNewsItemModel
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public DateTimeOffset PublishDate { get; set; }

        public string Text { get; set; }
    }
}
