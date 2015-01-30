using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Domain.Models
{
    public class NewLinkModel
    {
        [Url(ErrorMessage="Некорректный Url")]
        [Required(ErrorMessage="Не указана ссылка")]
        public string Url { get; set; }
        public int LinkId { get; set; }
        public int? CategoryId { get; set; }
        public string Title { get; set; }
        public DateTime DateWhenAdded { get; set; }
        public List<CategoryModel> Categories{ get; set; }
        public List<TagModel> Tags { get; set; }
        public string TagsInString { get; set; }
       
    }
}
