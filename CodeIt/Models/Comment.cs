﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CodeIt.Models
{

    //Model for the Comment used to store in DATABASE
    public class Comment
    {
        public Comment()
        {
            this.TimeCreated = DateTime.Now;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Code Comment")]
        [AllowHtml]
        public string Content { get; set; }

        public int CodeId { get; set; }

        public string AuthorId { get; set; }

        public virtual User Author { get; set; }

        public DateTime TimeCreated { get; set; }

        public bool IsAuthor(string authorId)
        {
            return this.AuthorId == authorId;
        }
    }
}