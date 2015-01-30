using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class RegistrationModel
    {
        [Required(ErrorMessage = "Введите имя")]
        [StringLength(50, ErrorMessage = "Длина имени не должна превышать 50 символов")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [StringLength(50, ErrorMessage = "Длина пароля не должна превышать 50 символов")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [Required(ErrorMessage = "Повторите пароль")]
        public string ConfirmPassword { get; set; }

        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                            @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                            @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
                            ErrorMessage = "Введите корректный E-mail")]
        [Required (ErrorMessage="E-mail не введен")]
        public string Email { get; set; }
    }
}
