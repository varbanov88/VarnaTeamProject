using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        public int PrevPage { get; set; }

        public List<Comment> Coments {get;set;}



    }
}