using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Введите имя")]
        [StringLength(50, ErrorMessage = "Длина имени не должна превышать 50 символов")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Введите пароль.")]
        [StringLength(50, ErrorMessage = "Длина пароля не должна превышать 50 символов")]
        public string Password { get; set; }
    }
}
