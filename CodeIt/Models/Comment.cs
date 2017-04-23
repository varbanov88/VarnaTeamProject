using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CodeIt.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public int CodeId { get; set; }

        [Required]
        public string AuthorId { get; set; }

        public virtual User Author { get; set; }
    }
}