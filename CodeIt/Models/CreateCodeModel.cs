using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CodeIt.Models
{
    //Model used for the Creating View of new Paste/Code
    public class CreateCodeModel
    {
        [Required(ErrorMessage = "Title is required")]
        [Display(Name = "Title")]
        [StringLength(200)]
        public string CodeTitle { get; set; }

        [Required(ErrorMessage = "Code is required")]
        [Display(Name = "Your Code")]
        [AllowHtml]
        public string CodeContent { get; set; }
    }
}