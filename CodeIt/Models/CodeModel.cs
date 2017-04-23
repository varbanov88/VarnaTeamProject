using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CodeIt.Models
{
    public class CodeModel
    {

        public CodeModel()
        {
            this.Comments = new List<string>();
        }


        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Code Title")]
        [StringLength(200)]
        public string CodeTitle { get; set; }

        [Required]
        public string CodeContent { get; set; }

        [Required]
        public string AuthorId { get; set; }

        public virtual User Author { get; set; }

        public List<string> Comments { get; set; }

    }
}