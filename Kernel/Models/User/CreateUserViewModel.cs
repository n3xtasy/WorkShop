using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WSAPI.Models.User
{
    public class CreateUserViewModel
    {
        [Required(ErrorMessage = "Не указан Email"), Display(Name = "Email Address"), DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Не указан пароль"), MinLength(10), DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Не указан пароль"), MinLength(10), DataType(DataType.Password), Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
