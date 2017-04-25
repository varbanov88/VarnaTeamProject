using System;
using System.ComponentModel.DataAnnotations;

namespace CodeIt.Models
{
    //Model used for Code pasted by Guest User. Used to store in DATABASE
    public class GuestCodeModel
    {
        public GuestCodeModel()
        {
            this.Author = "Guest";
            this.TimeCreated = DateTime.Now;
        }

        public int Id { get; set; }

        [Required]
        [Display(Name = "Code Title")]
        [StringLength(200)]
        public string CodeTitle { get; set; }

        [Required]
        [Display(Name = "Code Content")]
        public string CodeContent { get; set; }

        public string Author { get; set; }

        public DateTime TimeCreated { get; set; }
    }
}