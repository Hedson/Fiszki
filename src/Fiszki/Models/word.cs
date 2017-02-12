using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Fiszki.Models
{
    public class Word
    {
        public int ID { get; set; }

        [Display(Name = "Polish word:")]
        [Required(ErrorMessage = "You have to enter polish word:")]
        [StringLength(50)]
        public string PolishWord { get; set; }

        [Display(Name = "English word:")]
        [Required(ErrorMessage = "You have to enter english word:")]
        [StringLength(50)]
        public string EnglishWord { get; set; }

    }
}
