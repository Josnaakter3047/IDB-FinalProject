using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels.Auth.View
{
    public class RefreshTokenViewModel
    {
        [Required]
        public string RefreshToken { get; set; }

        [Required]
        public string ActualToken { get; set; }
    }
}
