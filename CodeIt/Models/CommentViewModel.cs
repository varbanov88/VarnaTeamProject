using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CodeIt.Models
{
    //Model used for the Creating View of the Comment
    public class CommentViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Code Comments")]
        public string Content { get; set; }

        public int CodeId { get; set; }

        public string AuthorId { get; set; }


        public virtual User Author { get; set; }

        public List<string> Code { get; set; }

        public DateTime TimeCreated { get; set; }
    }
}