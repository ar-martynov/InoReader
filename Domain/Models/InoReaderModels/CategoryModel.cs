using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Domain.Models;

namespace Domain.Models
{

    public class CategoryModel:SimpleCategoryModel
    {
        [Required(ErrorMessage="Введите название категории")]
        [RegularExpression("^[_a-zA-Z0-9а-яА-ЯёЁ ]+$", ErrorMessage = "Недопустимое название категории")]
        public string Title { get; set; }
        public List<LinkModel> Links { get; set; }
    }
}
