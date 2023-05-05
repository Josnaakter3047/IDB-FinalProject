using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels.Other.View
{
    public class IfTitleExistsViewModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; }
    }
}
