using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CodeIt.Models
{
    //Model used for The Detailed View of Code from Guest User
    public class GuestCodeDetailsModel
    {
        public GuestCodeDetailsModel()
        {
            this.Coments = new List<CommentOnGuest>();
        }


        public int Id { get; set; }

        [Display(Name = "Code Title")]
        public string CodeTitle { get; set; }

        [Display(Name = "Code")]
        public List<string> CodeContent { get; set; }

        [Display(Name = "Author")]
        public string Author { get; set; }

        public int PrevPage { get; set; }

        public List<CommentOnGuest> Coments { get; set; }

        public DateTime TimeCreated { get; set; }
    }
}