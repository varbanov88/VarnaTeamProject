using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CodeIt.Models
{

    //Model for the Comment used to store in DATABASE
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Code Comment")]
        public string Content { get; set; }

        public int CodeId { get; set; }

        public string AuthorId { get; set; }

        public virtual User Author { get; set; }

    }
}