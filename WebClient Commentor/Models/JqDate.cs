using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebClient_Commentor.Models
{
    public class JqDate
    {

        [Required]
        [Display(Name = "Vælg Start Dato")]
        public DateTime StartDate { get; set; }
        [Required]
        [Display(Name = "Vælg Slut Dato")]
        public DateTime EndDate { get; set; }
    }
}