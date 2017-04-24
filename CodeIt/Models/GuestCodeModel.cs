using System;
using System.ComponentModel.DataAnnotations;

namespace CodeIt.Models
{
    public class GuestCodeModel
    {
        public GuestCodeModel()
        {
            this.Author = "Guest";
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
    }
}