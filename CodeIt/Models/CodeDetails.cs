using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


//Used as a model for the Code Details View
namespace CodeIt.Models
{
    public class CodeDetails
    {
        public CodeDetails()
        {
            this.Coments = new List<Comment>();
        }


        public int Id { get; set; }

        [Display(Name = "Code Title")]
        public string CodeTitle { get; set; }

        [Display(Name = "Code")]
        public List<string> CodeContent { get; set; }

        [Display(Name = "Author")]

        public string Author { get; set; }

        public string ContactInfo { get; set; }

        public string AuthorId { get; set; }

        //Optional | tells the Details View The Previous Page
        public int PrevPage { get; set; }

        //Optional | if you are in My codes tell the user
        public string MyUser { get; set; }

        public List<Comment> Coments {get;set;}

        public bool isAuthor(string authorId)
        {
            return this.AuthorId == authorId;
        }

        public DateTime TimeCreated { get; set; }
    }
}