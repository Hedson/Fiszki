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
        [Required(ErrorMessage = "You have to enter player e-mail:")]
        [StringLength(50)]
        public string Email { get; set; }

        [Display(Name = "Points:")]
        [Required(ErrorMessage = "You have to enter amount of points:")]
        public int Points { get; set; }
    }
}
