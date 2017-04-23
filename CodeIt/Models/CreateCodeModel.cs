using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CodeIt.Models
{
    public class CreateCodeModel
    {
        [Required(ErrorMessage = "Title is required")]
        [Display(Name = "Title")]
        [StringLength(200)]
        public string CodeTitle { get; set; }

        [Required(ErrorMessage = "Code is required")]
        [Display(Name = "Your Code")]
        public string CodeContent { get; set; }


    }
}