using System;
using System.ComponentModel.DataAnnotations;

namespace CodeIt.Models
{
    //Model for Comment on Guest Paste used to store in DATABASE
    public class CommentOnGuest
    {
        public CommentOnGuest()
        {
            this.TimeCreated = DateTime.Now;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Code Comment")]
        public string Content { get; set; }

        public int CodeId { get; set; }

        public string AuthorId { get; set; }

        public virtual User Author { get; set; }

        public DateTime TimeCreated { get; set; }

        public bool IsAuthor (string authorId)
        {
            return this.AuthorId == authorId;
        }
    }
}