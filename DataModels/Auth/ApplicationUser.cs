using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels.Auth
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string FullName { get; set; }

        [ForeignKey("Parent")]
        public string ParentId { get; set; }
        public ApplicationUser Parent { get; set; }
        public DateTime? TimeStamp { get; set; }
        [Required]
        public string CustomerId { get; set; }
       
        public bool Active { get; set; }
        [NotMapped]
        public float RegistrationFee { get; set; }
        [NotMapped]
        public bool RegistrationFeePaid { get; set; }
        public ApplicationUser()
        {
            this.Active = false;
        }
    }
    
}
