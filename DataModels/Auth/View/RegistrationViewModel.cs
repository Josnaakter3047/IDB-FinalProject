using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels.Auth.View
{
    public class RegistrationViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [MaxLength(11)]
        [MinLength(11)]
        public string PhoneNumber { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }

        [Required]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@#$!%*?&])[A-Za-z\\d@#$!%*?&]{8,32}$",
            ErrorMessage = "Password Must Contain - One of Each [0-9], [a-z], [A-Z], Special Character in (@, #, $, !, %, *, ?, &) & 8-32 in length")]
        public string Password { get; set; }
      
    }
}
