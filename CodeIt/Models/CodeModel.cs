using System;
using System.ComponentModel.DataAnnotations;

namespace CodeIt.Models
{

    //Model for the Code used to store in DATABASE
    public class CodeModel
    {
        public CodeModel()
        {
            this.TimeCreated = DateTime.Now;
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title needed")]
        [Display(Name = "Code Title")]
        [StringLength(200)]
        public string CodeTitle { get; set; }

        [Required(ErrorMessage = "Content Needed")]
        public string CodeContent { get; set; }

        
        public string AuthorId { get; set; }

        public virtual User Author { get; set; }

        public bool IsAuthor (string authorId)
        {
            return this.AuthorId == authorId;
        }

        public DateTime TimeCreated { get; set; }
    }
}