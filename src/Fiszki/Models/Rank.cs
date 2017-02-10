using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Fiszki.Models
{
    public class Rank
    {
        public int ID { get; set; }

        [Display(Name = "Email:")]
        [StringLength(50)]
        public string Email { get; set; }

        [Display(Name = "Points:")]
        public int Points { get; set; }
    }
}
