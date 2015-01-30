using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.Syndication;


namespace Domain.Models
{
    public class RssCanalModel
    {
        public int RssCanalId { get; set; }
        [Url]
        [Required(ErrorMessage="Введите URL ресурса")]
        public string Url { get; set; }
        public string Title { get; set; }
        public List<RssNewsItemModel> RssNews { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public string ActionName { get; set; }

    }
}
