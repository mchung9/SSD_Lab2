using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SSD_Lab1.Models
{
    public class Team
    {
        //Primary key
        [Key]
        public int Id { get; set; }

        //Name of the team
        [Display(Name = "Team Name")]
        [Required]
        public string TeamName { get; set; }

        //Email addres of the team
        [Display(Name = "Email Address")]
        [Required(ErrorMessage ="The email addres is required")]
        [EmailAddress(ErrorMessage ="Invalid Email Address")]
        public string Email { get; set; }

        //Established data of the team
        [Display(Name = "Established Date")]
        public DateTime EstablishedDate { get; set; }


    }
}
